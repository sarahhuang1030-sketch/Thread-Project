// Custom email validation to match server-side validation
// This overrides the default jQuery validation for email to require a proper domain

$(document).ready(function () {
    // Override jQuery validator's email method
    if ($.validator && $.validator.methods.email) {
        $.validator.methods.email = function (value, element) {
            // Allow empty values (handled by required validation)
            if (!value || value.length === 0) {
                return true;
            }

            // Use the same regex as the server-side RegularExpression attribute
            // Must have: username@domain.tld format
            return /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(value);
        };
    }
});
