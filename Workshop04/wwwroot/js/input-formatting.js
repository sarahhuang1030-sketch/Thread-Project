// Phone Number Formatting (xxx-xxx-xxxx)
function formatPhoneNumber(input) {
    // Get cursor position before formatting
    let cursorPosition = input.selectionStart;
    let oldValue = input.value;

    // Remove all non-digits
    let value = input.value.replace(/\D/g, '');

    // Limit to 10 digits
    value = value.substring(0, 10);

    // Format as xxx-xxx-xxxx
    let formattedValue = '';
    if (value.length >= 6) {
        formattedValue = value.substring(0, 3) + '-' + value.substring(3, 6) + '-' + value.substring(6);
    } else if (value.length >= 3) {
        formattedValue = value.substring(0, 3) + '-' + value.substring(3);
    } else {
        formattedValue = value;
    }

    // Only update if the value actually changed
    if (input.value !== formattedValue) {
        input.value = formattedValue;

        // Adjust cursor position after formatting
        // Count how many dashes are before the cursor in the new value
        let dashesBeforeCursor = formattedValue.substring(0, cursorPosition).split('-').length - 1;
        let oldDashesBeforeCursor = oldValue.substring(0, cursorPosition).split('-').length - 1;

        // If we added or removed a dash, adjust cursor
        if (dashesBeforeCursor !== oldDashesBeforeCursor) {
            cursorPosition += (dashesBeforeCursor - oldDashesBeforeCursor);
        }

        // Set cursor position
        input.setSelectionRange(cursorPosition, cursorPosition);
    }
}

// Postal Code Formatting (A1A 1A1)
function formatPostalCode(input) {
    // Get cursor position before formatting
    let cursorPosition = input.selectionStart;
    let oldValue = input.value;

    // Remove all spaces and convert to uppercase
    let value = input.value.replace(/\s/g, '').toUpperCase();

    // Remove any invalid characters (keep only letters and numbers)
    value = value.replace(/[^A-Z0-9]/g, '');

    // Limit to 6 characters
    value = value.substring(0, 6);

    // Format as A1A 1A1
    let formattedValue = '';
    if (value.length > 3) {
        formattedValue = value.substring(0, 3) + ' ' + value.substring(3);
    } else {
        formattedValue = value;
    }

    // Only update if the value actually changed
    if (input.value !== formattedValue) {
        input.value = formattedValue;

        // Adjust cursor position after formatting
        let spacesBeforeCursor = formattedValue.substring(0, cursorPosition).split(' ').length - 1;
        let oldSpacesBeforeCursor = oldValue.substring(0, cursorPosition).split(' ').length - 1;

        // If we added or removed a space, adjust cursor
        if (spacesBeforeCursor !== oldSpacesBeforeCursor) {
            cursorPosition += (spacesBeforeCursor - oldSpacesBeforeCursor);
        }

        // Set cursor position
        input.setSelectionRange(cursorPosition, cursorPosition);
    }
}

// Apply formatting on page load
document.addEventListener('DOMContentLoaded', function () {
    // Phone number inputs - more specific selectors for Home and Business phone
    const phoneInputs = document.querySelectorAll(
        'input[name*="Phone"], input[id*="Phone"], ' +
        'input[name*="HomePhone"], input[id*="HomePhone"], ' +
        'input[name*="BusinessPhone"], input[id*="BusinessPhone"]'
    );
    
    phoneInputs.forEach(input => {
        // Format existing values on page load
        if (input.value) {
            formatPhoneNumber(input);
        }
        
        // Format as user types
        input.addEventListener('input', function () {
            formatPhoneNumber(this);
        });
        
        // Format on paste
        input.addEventListener('paste', function () {
            setTimeout(() => formatPhoneNumber(this), 10);
        });
    });

    // Postal code inputs
    const postalInputs = document.querySelectorAll('input[name*="Postal"], input[id*="Postal"]');
    postalInputs.forEach(input => {
        // Format existing values on page load
        if (input.value) {
            formatPostalCode(input);
        }
        
        // Format as user types
        input.addEventListener('input', function () {
            formatPostalCode(this);
        });
        
        // Format on paste
        input.addEventListener('paste', function () {
            setTimeout(() => formatPostalCode(this), 10);
        });
    });
});