const userIcon = document.querySelector('.fa-regular.fa-user');
const profile = document.querySelector('.profile');

let isProfileVisible = false; 

userIcon.addEventListener('click', function () {
    if (isProfileVisible) {
        profile.style.opacity = '0';
        setTimeout(() => {
            profile.style.visibility = 'hidden';
        }, 300); 
    } else {
        profile.style.visibility = 'visible';
        profile.style.opacity = '1';
    }

    isProfileVisible = !isProfileVisible; 
});


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

document.querySelector('.srt-btn').addEventListener('click', function () {
    document.querySelector('.sort form').style.visibility = 'visible';
});
$('.studio-carousel').owlCarousel({
    loop: true,
    margin: 10,
    nav: true,

    responsive: {
        0: {
            items: 1
        },
        768: {
            items: 3
        },
        1000: {
            items: 4
        }
    }
})
$('.cartoon-carousel').owlCarousel({
    loop: true,
    margin: 10,
    nav: true,
    items: 1,
    autoplay: true,
    autoplayTimeout: 5000,
    autoplayHoverPause: true,
    animateOut: 'fadeOut',
    animateIn: 'fadeIn',
    smartSpeed: 1000
});
$('.owl-cartoon-carousel').owlCarousel({
    loop: true,
    margin: 10,
    nav: true,
    autoplay: true,
    autoplayTimeout: 2000,
    responsive: {
        0: {
            items: 2
        },
        768: {
            items: 3
        },
        1000: {
            items: 6
        }
    }
})