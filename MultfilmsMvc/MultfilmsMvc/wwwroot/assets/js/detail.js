$('.studio-carousel').owlCarousel({
    loop:true,
    margin:10,
    nav:true,
       
    responsive:{
        0:{
            items:1
        },
        600:{
            items:2
        },
        1000:{
            items:4
        }
    }
})


const toggleBtn = document.querySelector('.menu-sidebar');
const sidebar = document.querySelector('.sidebar');

let sidebarVisible = false;

toggleBtn.addEventListener('click', function () {
    if (!sidebarVisible) {
        sidebar.style.visibility = 'visible';
        sidebar.style.opacity = '1';
        sidebar.style.transform = 'translateX(0)';
        sidebarVisible = true;
    } else {
        sidebar.style.opacity = '0';
        sidebar.style.transform = 'translateX(-100%)';
        setTimeout(() => {
            sidebar.style.visibility = 'hidden';
        }, 300);
        sidebarVisible = false;
    }
});
