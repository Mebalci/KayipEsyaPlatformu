$(document).on('click', '.mesaj-oku-btn', function () {
    var mesajId = $(this).data('id');
    $.get('/AdminPaneli/MesajDetay', { id: mesajId }, function(data) {
        console.log("Gelen mesaj:", data.mesaj);
        $('#mesajModalBody').html(data.mesaj);
        $('#mesajModal').modal('show');
        var badge = $('#mesajRow_' + mesajId + ' .okundu-badge');
        badge.removeClass('bg-danger').addClass('bg-success').text('Okundu');
    });
    $.post('/AdminPaneli/MesajOkunduYap', { id: mesajId });
});
$(document).on('click', '.okundu-badge', function () {
    var badge = $(this);
    var mesajId = badge.data('id');
    var okunduMu = badge.data('okundu') === true || badge.data('okundu') === "true";
    $.ajax({
        url: '/AdminPaneli/MesajDurumToggle',
        type: 'POST',
        data: { id: mesajId, okunduMu: okunduMu },
        success: function (result) {
            var yeniDurum = result.yeniDurum;
            badge
                .toggleClass('bg-success', yeniDurum)
                .toggleClass('bg-danger', !yeniDurum)
                .text(yeniDurum ? 'Okundu' : 'Okunmadı')
                .data('okundu', yeniDurum);
        },
        error: function () {
            alert('Bir hata oluştu.');
        }
    });
});
$(document).on('click', '.sil-btn', function () {
    if (!confirm('Bu mesajı silmek istediğinize emin misiniz?')) return;
    var btn = $(this);
    var mesajId = btn.data('id');
    $.ajax({
        url: '/AdminPaneli/MesajSil',
        type: 'POST',
        data: { id: mesajId },
        success: function (result) {
            if (result.success) {
                $('#mesajRow_' + mesajId).fadeOut(300, function () { $(this).remove(); });
            } else {
                alert('Silinemedi!');
            }
        },
        error: function () {
            alert('Bir hata oluştu.');
        }
    });
}); 