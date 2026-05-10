/* ── Navbar scroll effect ── */
const navbar = document.getElementById('navbar');
window.addEventListener('scroll', () => {
  navbar.classList.toggle('scrolled', window.scrollY > 40);
});

/* ── Intersection Observer: animate cards on scroll ── */
const observer = new IntersectionObserver((entries) => {
  entries.forEach((entry, i) => {
    if (entry.isIntersecting) {
      entry.target.style.animationDelay = `${i * 0.1}s`;
      entry.target.classList.add('visible');
      observer.unobserve(entry.target);
    }
  });
}, { threshold: 0.15 });

document.querySelectorAll(
  '.layer-card, .principle-card, .vg-card, .branch-card, .flow-step'
).forEach(el => {
  el.style.opacity = '0';
  el.style.transform = 'translateY(20px)';
  el.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
  observer.observe(el);
});

// Mark visible
const styleTag = document.createElement('style');
styleTag.textContent = `.visible { opacity: 1 !important; transform: translateY(0) !important; }`;
document.head.appendChild(styleTag);

/* ── Active nav link on scroll ── */
const sections = document.querySelectorAll('section[id]');
const navLinks = document.querySelectorAll('.nav-links a');

const sectionObserver = new IntersectionObserver((entries) => {
  entries.forEach(entry => {
    if (entry.isIntersecting) {
      navLinks.forEach(link => {
        link.style.color = '';
        if (link.getAttribute('href') === `#${entry.target.id}`) {
          link.style.color = 'var(--glow)';
        }
      });
    }
  });
}, { threshold: 0.4 });

sections.forEach(sec => sectionObserver.observe(sec));

/* ── Cursor glow effect ── */
const glow = document.createElement('div');
glow.style.cssText = `
  position: fixed; pointer-events: none; z-index: 9999;
  width: 300px; height: 300px; border-radius: 50%;
  background: radial-gradient(circle, rgba(139,92,246,0.06) 0%, transparent 70%);
  transform: translate(-50%, -50%);
  transition: opacity 0.3s;
`;
document.body.appendChild(glow);

document.addEventListener('mousemove', e => {
  glow.style.left = e.clientX + 'px';
  glow.style.top  = e.clientY + 'px';
});

/* ── Smooth typing animation for hero badge ── */
const badge = document.querySelector('.hero-badge');
if (badge) {
  const text = badge.textContent;
  badge.textContent = '';
  badge.style.opacity = '1';
  let i = 0;
  const typeInterval = setInterval(() => {
    badge.textContent += text[i++];
    if (i >= text.length) clearInterval(typeInterval);
  }, 35);
}

/* ── Layer card click expand detail ── */
document.querySelectorAll('.layer-card').forEach(card => {
  card.addEventListener('click', function () {
    this.style.zIndex = '10';
    this.style.position = 'relative';
  });
});

console.log(`
██████╗  █████╗ ███╗   ██╗██╗  ██╗ █████╗ ██╗
██╔══██╗██╔══██╗████╗  ██║██║ ██╔╝██╔══██╗██║
██████╔╝███████║██╔██╗ ██║█████╔╝ ███████║██║
██╔══██╗██╔══██║██║╚██╗██║██╔═██╗ ██╔══██║██║
██████╔╝██║  ██║██║ ╚████║██║  ██╗██║  ██║██║
╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝

bankai.se — Clean Architecture ASP.NET Core API
VG Level — 2026
`);
