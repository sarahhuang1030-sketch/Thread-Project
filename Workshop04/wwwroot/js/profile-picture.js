// Profile Picture Upload JavaScript
document.addEventListener('DOMContentLoaded', function() {
    const fileInput = document.getElementById('profile-picture-input');
    const preview = document.getElementById('profile-picture-preview');
    const removeCheckbox = document.getElementById('remove-picture-checkbox');
    
    if (fileInput) {
        fileInput.addEventListener('change', function(e) {
            const file = e.target.files[0];
            
            // Validate file
            if (file) {
                // Check file size (5MB)
                if (file.size > 5 * 1024 * 1024) {
                    alert('File size must be less than 5MB');
                    fileInput.value = '';
                    return;
                }
                
                // Check file type
                const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif'];
                if (!allowedTypes.includes(file.type.toLowerCase())) {
                    alert('Only JPG, PNG, and GIF files are allowed');
                    fileInput.value = '';
                    return;
                }
                
                // Show preview
                const reader = new FileReader();
                reader.onload = function(e) {
                    preview.src = e.target.result;
                    
                    // Add a subtle animation
                    preview.style.opacity = '0.5';
                    setTimeout(function() {
                        preview.style.transition = 'opacity 0.3s ease-in-out';
                        preview.style.opacity = '1';
                    }, 100);
                };
                reader.readAsDataURL(file);
                
                // Uncheck remove checkbox if checked
                if (removeCheckbox) {
                    removeCheckbox.checked = false;
                }
            }
        });
    }
    
    // Handle remove checkbox
    if (removeCheckbox) {
        removeCheckbox.addEventListener('change', function() {
            if (this.checked) {
                preview.src = '/images/default-avatar.svg';
                fileInput.value = '';
                
                // Add subtle animation
                preview.style.opacity = '0.5';
                setTimeout(function() {
                    preview.style.transition = 'opacity 0.3s ease-in-out';
                    preview.style.opacity = '1';
                }, 100);
            }
        });
    }

    // Optional: Add drag and drop functionality
    const profilePictureContainer = document.querySelector('.profile-picture-container');
    if (profilePictureContainer && fileInput) {
        ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
            profilePictureContainer.addEventListener(eventName, preventDefaults, false);
        });

        function preventDefaults(e) {
            e.preventDefault();
            e.stopPropagation();
        }

        ['dragenter', 'dragover'].forEach(eventName => {
            profilePictureContainer.addEventListener(eventName, highlight, false);
        });

        ['dragleave', 'drop'].forEach(eventName => {
            profilePictureContainer.addEventListener(eventName, unhighlight, false);
        });

        function highlight(e) {
            profilePictureContainer.style.opacity = '0.7';
            profilePictureContainer.style.transform = 'scale(1.05)';
        }

        function unhighlight(e) {
            profilePictureContainer.style.opacity = '1';
            profilePictureContainer.style.transform = 'scale(1)';
        }

        profilePictureContainer.addEventListener('drop', handleDrop, false);

        function handleDrop(e) {
            const dt = e.dataTransfer;
            const files = dt.files;

            if (files.length > 0) {
                fileInput.files = files;
                // Trigger change event
                const event = new Event('change', { bubbles: true });
                fileInput.dispatchEvent(event);
            }
        }
    }
});
