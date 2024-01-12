// Validation for Empty Input
function validateFormFields() {
    var isValid = true;

    var firstName = $('#firstName').val();
    var firstNameRegex = /^[a-zA-Z]+$/;
    if (firstName === '') {
        $('#firstNameError').text('First Name is Required').show();
        $('firstName').css('border')
        isValid = false;
    }
    else if (!firstNameRegex.test(firstName)) {
        $('#firstNameError').text('First Name can only contain letters.').show();
        isValid = false;
    }
    else {
        $('#firstNameError').text('').hide();
    }

    var lastName = $('#lastName').val();
    var lastNameRegex = /^[a-zA-Z]+$/;
    if (lastName === '') {
        $('#lastNameError').text('Last Name is Required').show();
        isValid = false;
    }
    else if (!lastNameRegex.test(lastName)) {
        $('#lastNameError').text('Last Name can only contain letters.').show();
        isValid = false;
    }
    else {
        $('#lastNameError').text('').hide();
    }

    var emailId = $('#emailId').val();
    var emailIdRegex = /^[0-9a-zA-Z]+[+._-]{0,1}[0-9a-zA-Z]+[@][a-zA-Z0-9]+[.][a-zA-Z]{2,3}([.][a-zA-Z]{2,3}){0,1}$/;
    if (emailId === '') {
        $('#emailIdError').text('Email is Required').show();
        isValid = false;
    }
    else if (!emailIdRegex.test(emailId)) {
        $('#emailIdError').text('Please Enter Valid Email Id.').show();
        isValid = false;
    }
    else {
        $('#emailIdError').text('').hide();
    }

    var contactNo = $('#contactNo').val();
    var contactNoRegex = /^(?:\+91|91|0)?[789]\d{9}$/;
    if (contactNo === '') {
        $('#contactNoError').text('Contact No. is Required').show();
        isValid = false;
    }
    else if (!contactNoRegex.test(contactNo)) {
        $('#contactNoError').text('Please Enter Valid Contact Number').show();
        isValid = false;
    }
    else {
        $('#contactNoError').text('').hide();
    }

    var age = $('#age').val();
    var ageRegex = /^[1-9][0-9]$/;
    if (age === '') {
        $('#ageError').text('Age is Required').show();
        isValid = false;
    }
    else if (!ageRegex.test(age)) {
        $('#ageError').text('Please Enter Valid Age.').show();
        isValid = false;
    }
    else {
        $('#ageError').text('').hide();
    }

    return isValid;
}

// type something and hide error message
$('#firstName').on('input', function () {
    var firstName = $(this).val();
    if (firstName !== '') {
        $('#firstNameError').text('').hide();
    }
});

$('#lastName').on('input', function () {
    var lastName = $(this).val();
    if (lastName !== '') {
        $('#lastNameError').text('').hide();
    }
});

$('#emailId').on('input', function () {
    var emailId = $(this).val();
    if (emailId !== '') {
        $('#emailIdError').text('').hide();
    }
});

$('#contactNo').on('input', function () {
    var contactNo = $(this).val();
    if (contactNo !== '') {
        $('#contactNoError').text('').hide();
    }
});

$('#age').on('input', function () {
    var age = $(this).val();
    if (age !== '') {
        $('#ageError').text('').hide();
    }
});

function removeValidationError() {

    // Normalize input tags
    $('#firstName').css('border-color', '');
    $('#lastName').css('border-color', '');
    $('#emailId').css('border-color', '');
    $('#contactNo').css('border-color', '');
    $('#age').css('border-color', '');

    // Clear previous error messages
    $('#firstNameError').text('').hide();
    $('#lastNameError').text('').hide();
    $('#emailIdError').text('').hide();
    $('#contactNoError').text('').hide();
    $('#ageError').text('').hide();
}