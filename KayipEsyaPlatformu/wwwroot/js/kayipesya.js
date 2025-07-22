// DOM yüklendiğinde çalışacak şekilde kodu sarmalıyorum
window.addEventListener('DOMContentLoaded', function() {
    var kayipForm = document.getElementById("kayipForm");
    if (kayipForm) {
        kayipForm.addEventListener("submit", function (e) {
            e.preventDefault();

            const formData = Object.fromEntries(new FormData(this).entries());

            fetch("/KayipEsya/Ekle", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(formData)
            })
                .then(res => res.json())
                .then(data => {
                    if (data.success) {
                        document.getElementById("mesajKutusu").classList.remove("d-none");
                        document.getElementById("kayipForm").reset();
                    } else {
                        alert(data.mesaj || "Bir hata oluştu.");
                    }
                });
        });
    }
});


