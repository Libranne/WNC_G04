document.addEventListener('DOMContentLoaded', function () {
    const prevBtn = document.getElementById('prevBtn');
    const nextBtn = document.getElementById('nextBtn');
    const track = document.querySelector('.carousel-track');
    const items = document.querySelectorAll('.carousel-item');
    let currentIndex = 0;
    const itemsPerSlide = 5; // Hiển thị 5 chủ đề mỗi lần

    // Show next set of items (5 chủ đề)
    function showNext() {
        currentIndex += itemsPerSlide;
        if (currentIndex >= items.length) {
            currentIndex = 0; // Nếu đã đến cuối, quay lại đầu
        }
        updateCarousel();
    }

    // Show previous set of items (5 chủ đề)
    function showPrev() {
        currentIndex -= itemsPerSlide;
        if (currentIndex < 0) {
            currentIndex = Math.max(0, items.length - itemsPerSlide); // Nếu đã quay lại đầu, dừng ở vị trí cuối
        }
        updateCarousel();
    }

    // Update carousel
    function updateCarousel() {
        track.style.transform = `translateX(-${currentIndex * (100 / itemsPerSlide)}%)`;
    }

    prevBtn.addEventListener('click', showPrev);
    nextBtn.addEventListener('click', showNext);

    // Auto slide every 5 seconds (optional)
    setInterval(showNext, 5000);
});
