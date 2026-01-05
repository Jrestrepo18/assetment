// OnlineCoursesPlatform - JavaScript

(function () {
    'use strict';

    // Initialize Bootstrap tooltips
    document.addEventListener('DOMContentLoaded', function () {
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    });

    // Auto-hide alerts after 5 seconds
    document.addEventListener('DOMContentLoaded', function () {
        var alerts = document.querySelectorAll('.alert-dismissible');
        alerts.forEach(function (alert) {
            setTimeout(function () {
                var bsAlert = new bootstrap.Alert(alert);
                bsAlert.close();
            }, 5000);
        });
    });

    // Form validation styling
    document.addEventListener('DOMContentLoaded', function () {
        var forms = document.querySelectorAll('form');
        forms.forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                form.classList.add('was-validated');
            });
        });
    });

    // Confirm delete actions
    window.confirmDelete = function (message) {
        return confirm(message || '¿Está seguro de que desea eliminar este elemento?');
    };

    // Loading spinner
    window.showLoading = function () {
        var spinner = document.createElement('div');
        spinner.className = 'spinner-overlay';
        spinner.id = 'loadingSpinner';
        spinner.innerHTML = '<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Cargando...</span></div>';
        document.body.appendChild(spinner);
    };

    window.hideLoading = function () {
        var spinner = document.getElementById('loadingSpinner');
        if (spinner) {
            spinner.remove();
        }
    };

    // Character counter for textareas
    document.addEventListener('DOMContentLoaded', function () {
        var textareas = document.querySelectorAll('textarea[maxlength]');
        textareas.forEach(function (textarea) {
            var maxLength = textarea.getAttribute('maxlength');
            var counter = document.createElement('small');
            counter.className = 'text-muted d-block text-end';
            counter.textContent = '0 / ' + maxLength + ' caracteres';
            textarea.parentNode.appendChild(counter);

            textarea.addEventListener('input', function () {
                counter.textContent = textarea.value.length + ' / ' + maxLength + ' caracteres';
            });
        });
    });

    console.log('OnlineCoursesPlatform initialized');
})();
