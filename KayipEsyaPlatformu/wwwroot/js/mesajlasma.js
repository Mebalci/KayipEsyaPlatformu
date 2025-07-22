
var chatConnection = null;
var currentEslesmeId = null;
var currentUserId = null;
var currentDigerKullaniciAd = null;
var currentDigerKullaniciSoyad = null;
var currentKendiAd = null;
var currentKendiSoyad = null;

if (typeof toastr === 'undefined') {
    var toastrScript = document.createElement('script');
    toastrScript.src = 'https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js';
    document.head.appendChild(toastrScript);
    var toastrCss = document.createElement('link');
    toastrCss.rel = 'stylesheet';
    toastrCss.href = 'https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css';
    document.head.appendChild(toastrCss);
}

function updateMesajlarimBadge(eslesmeId) {
    
    var badge = $("#mesaj-badge-" + eslesmeId);
    if (badge.length) {
        var sayi = parseInt(badge.text()) || 0;
        badge.text(sayi + 1).show();
    }
}

function getInitials(ad, soyad) {
    return (ad && ad[0] ? ad[0] : '') + (soyad && soyad[0] ? soyad[0] : '');
}

function registerMesajBildirim(connection) {
    connection.on("MesajBildirimAl", function (eslesmeId, gonderenAd, mesaj, tarih) {
        toastr.options = {
            closeButton: true,
            progressBar: true,
            positionClass: "toast-bottom-right",
            timeOut: "6000"
        };
        toastr.info(`<b>${gonderenAd}</b>: ${mesaj}`, "Yeni Mesaj");
        updateMesajlarimBadge(eslesmeId);
    });
}

function initChat(eslesmeId, userId, digerKullaniciAd, digerKullaniciSoyad, kendiAd, kendiSoyad) {
    currentEslesmeId = eslesmeId;
    currentUserId = userId;
    currentDigerKullaniciAd = digerKullaniciAd;
    currentDigerKullaniciSoyad = digerKullaniciSoyad;
    currentKendiAd = kendiAd;
    currentKendiSoyad = kendiSoyad;

    if (chatConnection) {
        chatConnection.stop();
        chatConnection = null;
    }
    chatConnection = new signalR.HubConnectionBuilder().withUrl("/bildirimHub").build();
    chatConnection.start().then(function () {
        chatConnection.invoke("JoinMatchGroup", eslesmeId);
    });
    chatConnection.on("ReceiveMessage", function (gonderenId, mesaj, tarih) {
        var isMe = gonderenId === userId;
        var ad = isMe ? currentKendiAd : currentDigerKullaniciAd;
        var soyad = isMe ? currentKendiSoyad : currentDigerKullaniciSoyad;
        var avatar = getInitials(ad, soyad);
        var bubble = `<div class="chat-message ${isMe ? 'me' : ''}">
            <div class="message-avatar">${avatar}</div>
            <div class="message-content">
                <div class="message-bubble">${mesaj}</div>
                <div class="message-time">${isMe ? 'Siz' : ad} - ${new Date(tarih).toLocaleString('tr-TR')}</div>
            </div>
        </div>`;
        $('#chatMessages').append(bubble);
        var chatMessages = document.getElementById('chatMessages');
        chatMessages.scrollTop = chatMessages.scrollHeight;
    });
    registerMesajBildirim(chatConnection);
}

$(document).off('click', '#btnSendMessage').on('click', '#btnSendMessage', sendMessage);
$(document).off('keypress', '#chatInput').on('keypress', '#chatInput', function(e) {
    if (e.which === 13) sendMessage();
});

function sendMessage() {
    var mesaj = $('#chatInput').val().trim();
    if (!mesaj || !chatConnection) return;
    $('#btnSendMessage').prop('disabled', true);
    chatConnection.invoke("SendMessageToMatchGroup", currentEslesmeId, currentUserId, mesaj).then(function () {
        $('#chatInput').val('');
        $('#btnSendMessage').prop('disabled', false);
    });
     
    $.post('/Mesajlasma/MesajGonder', { eslesmeId: currentEslesmeId, icerik: mesaj });
}

// Modal kapatılırken gruptan çık
$(document).on('hidden.bs.modal', '#chatModal', function () {
    if (chatConnection && currentEslesmeId) {
        chatConnection.invoke("LeaveMatchGroup", currentEslesmeId);
        chatConnection.stop();
        chatConnection = null;
    }
});

// Kullanıcı paneli açıldığında userId ile JoinGroup
$(function() {
    var userId = window.currentUserId || $('body').data('userid') || $('[name="userId"]').val();
    if (userId) {
        var genelConnection = new signalR.HubConnectionBuilder().withUrl("/bildirimHub").build();
        genelConnection.start().then(function () {
            genelConnection.invoke("JoinGroup", userId);
        });
        registerMesajBildirim(genelConnection);
    }
}); 