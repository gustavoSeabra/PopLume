document.addEventListener("DOMContentLoaded", () => {
    
    /* --------------------------------------------------------
       1. Intersection Observer for Section Reveal Animations
       -------------------------------------------------------- */
    const revealSections = () => {
        const observerOptions = {
            root: null, // viewport
            rootMargin: "0px",
            threshold: 0.15 // trigger when 15% of section is visible
        };

        const observer = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add("show");
                    observer.unobserve(entry.target); // Stop observing once animated
                }
            });
        }, observerOptions);

        // Observe all sections
        const sections = document.querySelectorAll("section");
        sections.forEach(section => {
            // Hero section is shown instantly by default, others start hidden
            if (!section.classList.contains("hero-section")) {
                observer.observe(section);
            } else {
                section.classList.add("show");
            }
        });
    };

    /* --------------------------------------------------------
       2. Staggered Delay for Category Cards
       -------------------------------------------------------- */
    const staggerCategoryCards = () => {
        const cards = document.querySelectorAll(".category-card");
        cards.forEach((card, index) => {
            // Apply delay based on index for a cascading entry effect
            card.style.transitionDelay = `${index * 0.1}s`;
        });
    };

    /* --------------------------------------------------------
       3. Newsletter Form Interaction & Validation
       -------------------------------------------------------- */
    const handleNewsletterSubmit = () => {
        const form = document.getElementById("newsletterForm");
        if (!form) return;

        form.addEventListener("submit", (e) => {
            e.preventDefault();

            const emailInput = document.getElementById("newsletterEmail");
            const submitBtn = form.querySelector(".btn-submit");
            const originalText = submitBtn.textContent;

            if (!emailInput || !emailInput.value.trim()) return;

            // Simple email regex validation
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(emailInput.value.trim())) {
                emailInput.focus();
                emailInput.style.borderColor = "#FF3333";
                setTimeout(() => {
                    emailInput.style.borderColor = "";
                }, 2000);
                return;
            }

            // Visual feedback loop
            submitBtn.disabled = true;
            submitBtn.textContent = "Cadastrando...";
            submitBtn.style.backgroundColor = "var(--primary)";

            // Simulate API Request
            setTimeout(() => {
                // Success State
                submitBtn.textContent = "Inscrito! 🎉";
                submitBtn.style.backgroundColor = "#0F5F63"; // Success state from the brand palette
                emailInput.value = "";

                // Reset form button after a few seconds
                setTimeout(() => {
                    submitBtn.disabled = false;
                    submitBtn.textContent = originalText;
                    submitBtn.style.backgroundColor = "";
                }, 4000);

            }, 1200);
        });
    };

    /* --------------------------------------------------------
       4. Smooth Anchor Link Navigation Offset
       -------------------------------------------------------- */
    const initSmoothScroll = () => {
        document.querySelectorAll('a[href^="#"]').forEach(anchor => {
            anchor.addEventListener("click", function(e) {
                const targetId = this.getAttribute("href");
                if (targetId === "#") return;
                
                const targetElement = document.querySelector(targetId);
                if (targetElement) {
                    e.preventDefault();
                    
                    const headerHeight = 80; // approximate navbar size
                    const targetPosition = targetElement.getBoundingClientRect().top + window.pageYOffset - headerHeight;
                    
                    window.scrollTo({
                        top: targetPosition,
                        behavior: "smooth"
                    });
                }
            });
        });
    };

    // Initialize all components
    staggerCategoryCards();
    revealSections();
    handleNewsletterSubmit();
    initSmoothScroll();
});
