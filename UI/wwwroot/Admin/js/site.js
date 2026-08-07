$(document).ready(function () {
    // Sidebar active link highlighting based on current URL
    var currentUrl = window.location.pathname;
    $('.sf-sidebar-nav a').each(function () {
        var href = $(this).attr('href');
        if (href && currentUrl.toLowerCase().includes(href.toLowerCase()) && href !== '#') {
            $('.sf-sidebar-nav a').removeClass('active');
            $(this).addClass('active');
        } else if (currentUrl === '/' && href === '/') {
            $(this).addClass('active');
        }
    });

    // Mobile sidebar toggle
    $('.sf-sidebar-toggle').on('click', function () {
        $('.sf-sidebar').toggleClass('show');
    });

    // Search/filter bar reset button handler
    $('#btnResetSearch').on('click', function (e) {
        e.preventDefault();
        var form = $(this).closest('form');
        form.find('input[type="text"]').val('');
        form.find('select').prop('selectedIndex', 0);
        form.submit();
    });

    // General UI interactions (e.g. tooltips)
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
});
