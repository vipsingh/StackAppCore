
function validateField(controlInfo, value) {
    if (controlInfo.IsRequired) {
        return validator.required(controlInfo, value);
    }
}

const validator = {
    required: (controlInfo, value) => {
        if (!value) {
            return {
                IsValid: false,
                Message: "Field is required"
            };
        }
    }
};

export default { validateField };