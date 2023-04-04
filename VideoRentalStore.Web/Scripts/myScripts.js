var padLeft = function (value, width, char) {
    char = (char !== undefined) ? char : " ";
    width = (typeof width === "number") ? width : parseInt(width);
    value = String(value);
    return Array(1 + width - value.length).join(char) + value;
};
