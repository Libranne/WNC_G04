document.getElementById('showMoreBtn').addEventListener('click', function () {
    var menu = document.getElementById('moreOptions');
    // Toggle the visibility of the menu
    if (menu.style.display === 'none' || menu.style.display === '') {
        menu.style.display = 'block';
    } else {
        menu.style.display = 'none';
    }
});
$(document).ready(function () {
    // Toggle menu
    $('.navbar-toggle').on('click', function () {
        $('.header-navbar').toggleClass('active');
    });

    // Đóng menu khi click bên ngoài
    $(document).on('click', function (e) {
        if (!$(e.target).closest('.header-navbar, .navbar-toggle').length) {
            $('.header-navbar').removeClass('active');
        }
    });
});
$('.navbar-toggle').on('click', function () {
    $(this).find('i').toggleClass('fa-bars fa-times'); // Đổi icon
});

