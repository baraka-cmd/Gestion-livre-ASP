/* ============================================================
   BIBLIO-TECH UAGO - JAVASCRIPT COMPLET
   Compatible Visual Studio 2012 | JavaScript ES3
   Fonctionnalités : Navbar responsive, Back to Top, Recherche,
   Confirmations, Animations, Notifications
   ============================================================ */

/**
 * Attendre que le DOM soit complètement chargé
 */
document.addEventListener("DOMContentLoaded", function() {

    /* =========================================================
       1. NAVBAR RESPONSIVE - BOUTON TOGGLE
       ========================================================= */
    var navbarToggle = document.querySelector(".navbar-toggle");
    var navbarCollapse = document.getElementById("navbar-collapse-main");

    if (navbarToggle && navbarCollapse) {
        // S'assurer que le menu est fermé au départ sur mobile
        navbarCollapse.classList.remove("in");
        navbarCollapse.style.display = "none";

        // Gestion du clic sur le bouton hamburger
        navbarToggle.addEventListener("click", function() {
            if (navbarCollapse.style.display === "none" || navbarCollapse.style.display === "") {
                navbarCollapse.style.display = "block";
                navbarCollapse.classList.add("in");
            } else {
                navbarCollapse.style.display = "none";
                navbarCollapse.classList.remove("in");
            }
        });

        // Fermer le menu si on clique en dehors (sur mobile)
        document.addEventListener("click", function(event) {
            var isClickInsideNav = navbarToggle.contains(event.target) || navbarCollapse.contains(event.target);
            if (!isClickInsideNav && window.innerWidth <= 768) {
                navbarCollapse.style.display = "none";
                navbarCollapse.classList.remove("in");
            }
        });

        // Gérer le redimensionnement de la fenêtre
        window.addEventListener("resize", function() {
            if (window.innerWidth > 768) {
                navbarCollapse.style.display = "block";
                navbarCollapse.classList.remove("in");
            } else {
                navbarCollapse.style.display = "none";
                navbarCollapse.classList.remove("in");
            }
        });

        // État initial selon la largeur
        if (window.innerWidth > 768) {
            navbarCollapse.style.display = "block";
        }
    }

    /* =========================================================
       2. BOUTON RETOUR EN HAUT (BACK TO TOP)
       ========================================================= */
    var backToTopBtn = document.getElementById("backToTop");

    if (backToTopBtn) {
        // Afficher/Masquer le bouton selon le défilement
        window.addEventListener("scroll", function() {
            if (document.body.scrollTop > 200 || document.documentElement.scrollTop > 200) {
                backToTopBtn.style.display = "block";
            } else {
                backToTopBtn.style.display = "none";
            }
        });

        // Clic pour remonter en haut (animation smooth)
        backToTopBtn.addEventListener("click", function() {
            scrollToTop(500);
        });

        // Cacher au chargement
        backToTopBtn.style.display = "none";
    }

    /* =========================================================
       3. FERMETURE AUTOMATIQUE DES ALERTES
       ========================================================= */
    var alerts = document.querySelectorAll(".alert:not(.alert-permanent)");
    if (alerts.length > 0) {
        for (var i = 0; i < alerts.length; i++) {
            (function(alert) {
                setTimeout(function() {
                    fadeOut(alert, 400, function() {
                        if (alert.parentNode) {
                            alert.parentNode.removeChild(alert);
                        }
                    });
                }, 5000);
            })(alerts[i]);
        }
    }

    /* =========================================================
       4. ANIMATION DES CARTES STATISTIQUES AU DÉFILEMENT
       ========================================================= */
    var statCards = document.querySelectorAll(".card-stat, .stat-card-inner, .stats-card");
    if (statCards.length > 0) {
        // Ajouter un léger délai progressif pour un effet cascade
        for (var j = 0; j < statCards.length; j++) {
            statCards[j].style.opacity = "0";
            statCards[j].style.transition = "opacity 0.6s ease, transform 0.6s ease";
            statCards[j].style.transform = "translateY(20px)";

            (function(card, index) {
                setTimeout(function() {
                    card.style.opacity = "1";
                    card.style.transform = "translateY(0)";
                }, 100 * index);
            })(statCards[j], j);
        }
    }

    /* =========================================================
       5. RECHERCHE DYNAMIQUE DANS LES TABLEAUX
       ========================================================= */
    var searchInputs = document.querySelectorAll("[id*='search'], [id*='Search'], [id*='txtSearch']");
    
    for (var k = 0; k < searchInputs.length; k++) {
        (function(input) {
            input.addEventListener("keyup", function() {
                var filter = input.value.toLowerCase();
                // Trouver le tableau le plus proche
                var table = findClosestTable(input);
                if (table) {
                    var rows = table.getElementsByTagName("tr");
                    var visibleCount = 0;
                    // Commencer à 1 pour sauter l'en-tête
                    for (var r = 1; r < rows.length; r++) {
                        var text = rows[r].textContent.toLowerCase();
                        if (text.indexOf(filter) > -1) {
                            rows[r].style.display = "";
                            visibleCount++;
                        } else {
                            rows[r].style.display = "none";
                        }
                    }
                    // Mettre à jour le compteur si présent
                    updateCounter(input, visibleCount);
                    // Afficher/Masquer le message "aucun résultat"
                    toggleNoResult(input, visibleCount, rows.length - 1);
                }
            });
        })(searchInputs[k]);
    }

    /* =========================================================
       6. CONFIRMATION DE SUPPRESSION
       ========================================================= */
    var deleteLinks = document.querySelectorAll("a[href*='Delete'], a[href*='delete'], a[onclick*='confirm']");
    
    for (var d = 0; d < deleteLinks.length; d++) {
        (function(link) {
            var originalOnClick = link.getAttribute("onclick");
            if (originalOnClick && originalOnClick.indexOf("confirm") >= 0) {
                // Déjà géré par l'attribut onclick inline
                return;
            }
            link.addEventListener("click", function(event) {
                var message = "Êtes-vous sûr de vouloir supprimer cet élément ? Cette action est irréversible.";
                if (!confirm(message)) {
                    event.preventDefault();
                    return false;
                }
            });
        })(deleteLinks[d]);
    }

    /* =========================================================
       7. ANIMATION DES LABELS AU FOCUS DES CHAMPS
       ========================================================= */
    var formControls = document.querySelectorAll(".form-control:not([readonly])");
    
    for (var f = 0; f < formControls.length; f++) {
        (function(control) {
            control.addEventListener("focus", function() {
                var formGroup = control.closest(".form-group, .form-item");
                if (formGroup) {
                    var label = formGroup.querySelector("label");
                    if (label) {
                        label.style.color = "#3498db";
                        label.style.transition = "color 0.3s ease";
                    }
                }
            });

            control.addEventListener("blur", function() {
                var formGroup = control.closest(".form-group, .form-item");
                if (formGroup) {
                    var label = formGroup.querySelector("label");
                    if (label) {
                        label.style.color = "";
                    }
                }
            });
        })(formControls[f]);
    }

    /* =========================================================
       8. NOTIFICATION DE MODIFICATION (PAGES D'ÉDITION)
       ========================================================= */
    var editForms = document.querySelectorAll("[id*='Edit'], [id*='edit'], [id*='frmEdit']");
    
    for (var e = 0; e < editForms.length; e++) {
        var inputs = editForms[e].querySelectorAll("input:not([readonly]):not([type='hidden']), select, textarea");
        var statusBadge = document.querySelector(".status-mode, .status-badge-edit, .status-mode-edit");
        var originalStatusText = statusBadge ? statusBadge.textContent : "";
        var originalStatusBg = statusBadge ? statusBadge.style.backgroundColor : "";

        for (var inp = 0; inp < inputs.length; inp++) {
            (function(input) {
                input.addEventListener("change", function() {
                    if (statusBadge) {
                        statusBadge.textContent = "Modifié";
                        statusBadge.style.backgroundColor = "#e74c3c";
                        statusBadge.style.transition = "background-color 0.3s ease";
                    }
                });
            })(inputs[inp]);
        }

        // Restaurer si le formulaire est réinitialisé
        var resetBtn = editForms[e].querySelector("button[type='reset'], .btn-action.default");
        if (resetBtn && statusBadge) {
            resetBtn.addEventListener("click", function() {
                setTimeout(function() {
                    statusBadge.textContent = originalStatusText;
                    statusBadge.style.backgroundColor = originalStatusBg;
                }, 100);
            });
        }
    }

    /* =========================================================
       9. BOUTON IMPRIMER (si présent)
       ========================================================= */
    var printButtons = document.querySelectorAll(".btn-print, .btn-print-outline");
    
    for (var p = 0; p < printButtons.length; p++) {
        (function(btn) {
            if (!btn.getAttribute("onclick") || btn.getAttribute("onclick").indexOf("print") < 0) {
                btn.addEventListener("click", function() {
                    window.print();
                });
            }
        })(printButtons[p]);
    }

    /* =========================================================
       10. DROPDOWN MENU (NAVBAR) - OUVERTURE AU CLIC SUR MOBILE
       ========================================================= */
    var dropdownToggles = document.querySelectorAll(".dropdown-toggle");
    
    for (var dt = 0; dt < dropdownToggles.length; dt++) {
        (function(toggle) {
            toggle.addEventListener("click", function(event) {
                if (window.innerWidth <= 768) {
                    event.preventDefault();
                    var dropdownMenu = toggle.nextElementSibling;
                    if (dropdownMenu && dropdownMenu.classList.contains("dropdown-menu")) {
                        if (dropdownMenu.style.display === "block") {
                            dropdownMenu.style.display = "none";
                        } else {
                            dropdownMenu.style.display = "block";
                        }
                    }
                }
            });
        })(dropdownToggles[dt]);
    }

});

/* ============================================================
   FONCTIONS UTILITAIRES
   ============================================================ */

/**
 * Défilement fluide vers le haut
 * @param {number} duration - Durée de l'animation en ms
 */
function scrollToTop(duration) {
    var start = document.body.scrollTop || document.documentElement.scrollTop;
    var startTime = null;

    function animate(currentTime) {
        if (startTime === null) {
            startTime = currentTime;
        }
        var elapsed = currentTime - startTime;
        var progress = Math.min(elapsed / duration, 1);
        // Fonction d'easing
        var easeInOutCubic = progress < 0.5 ? 4 * progress * progress * progress : (progress - 1) * (2 * progress - 2) * (2 * progress - 2) + 1;
        
        window.scrollTo(0, start * (1 - easeInOutCubic));
        
        if (elapsed < duration) {
            requestAnimationFrame(animate);
        }
    }

    requestAnimationFrame(animate);
}

/**
 * Fonction de fondu sortant
 * @param {HTMLElement} element - L'élément à animer
 * @param {number} duration - Durée en ms
 * @param {Function} callback - Fonction à exécuter après l'animation
 */
function fadeOut(element, duration, callback) {
    var opacity = 1;
    var interval = 20;
    var steps = duration / interval;
    var decrement = 1 / steps;

    var fadeInterval = setInterval(function() {
        opacity -= decrement;
        if (opacity <= 0) {
            opacity = 0;
            clearInterval(fadeInterval);
            element.style.opacity = opacity;
            if (typeof callback === "function") {
                callback();
            }
        }
        element.style.opacity = opacity;
    }, interval);
}

/**
 * Fonction de fondu entrant
 * @param {HTMLElement} element - L'élément à animer
 * @param {number} duration - Durée en ms
 */
function fadeIn(element, duration) {
    var opacity = 0;
    var interval = 20;
    var steps = duration / interval;
    var increment = 1 / steps;
    element.style.opacity = 0;
    element.style.display = "block";

    var fadeInterval = setInterval(function() {
        opacity += increment;
        if (opacity >= 1) {
            opacity = 1;
            clearInterval(fadeInterval);
        }
        element.style.opacity = opacity;
    }, interval);
}

/**
 * Trouver le tableau le plus proche d'un élément
 * @param {HTMLElement} element 
 * @returns {HTMLElement|null}
 */
function findClosestTable(element) {
    var current = element;
    // Remonter jusqu'à un conteneur commun
    while (current && current !== document.body) {
        var table = current.querySelector("table");
        if (table) {
            return table;
        }
        current = current.parentElement;
    }
    // Fallback : chercher toutes les tables
    var tables = document.querySelectorAll("table");
    if (tables.length > 0) {
        return tables[0];
    }
    return null;
}

/**
 * Mettre à jour le compteur d'éléments visibles
 * @param {HTMLElement} input - Le champ de recherche
 * @param {number} count - Nombre d'éléments visibles
 */
function updateCounter(input, count) {
    var counterSpan = document.getElementById("authCount");
    if (!counterSpan) {
        // Chercher un élément proche qui pourrait servir de compteur
        var container = input.closest(".well, .quick-stats-bar, .action-header");
        if (container) {
            var strongTags = container.querySelectorAll("strong");
            for (var i = 0; i < strongTags.length; i++) {
                if (strongTags[i].textContent.indexOf("Total") >= 0 || strongTags[i].parentElement.textContent.indexOf("Total") >= 0) {
                    counterSpan = strongTags[i];
                }
            }
        }
    }
    if (counterSpan) {
        counterSpan.textContent = count;
    }
}

/**
 * Afficher/Masquer le message "aucun résultat"
 * @param {HTMLElement} input - Le champ de recherche
 * @param {number} visibleCount - Nombre d'éléments visibles
 * @param {number} totalRows - Nombre total de lignes
 */
function toggleNoResult(input, visibleCount, totalRows) {
    var noResultDiv = document.getElementById("noResult");
    if (!noResultDiv) {
        // Chercher un élément proche
        var container = input.closest(".row, .well, .panel-body, .form-content-body");
        if (container) {
            noResultDiv = container.querySelector(".alert-warning, [id*='noResult'], [id*='no-result']");
        }
    }
    if (noResultDiv) {
        if (visibleCount === 0 && totalRows > 0) {
            noResultDiv.style.display = "block";
        } else {
            noResultDiv.style.display = "none";
        }
    }
}

/**
 * Fonction de suppression avec confirmation (appelée depuis onclick inline)
 * @param {string} id - Identifiant de l'élément à supprimer
 * @param {string} url - URL de base pour la suppression
 */
function confirmDelete(id, url) {
    var message = "ATTENTION : Voulez-vous vraiment supprimer l'élément n° " + id + " ? Cette action est irréversible.";
    if (confirm(message)) {
        window.location.href = url + "/" + id;
    }
}

/**
 * Redirection après un délai (pour les pages de notification)
 * @param {string} url - URL de destination
 * @param {number} delay - Délai en millisecondes
 */
function redirectAfter(url, delay) {
    if (!delay) delay = 3000;
    setTimeout(function() {
        window.location.href = url;
    }, delay);
}