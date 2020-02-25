import _ from "lodash";

const operators = {
    None: -1,
    Equal: 0,
    NotEqual: 1,
    GreaterThan: 3,
    LessThan: 4,
    GreaterThanEqual: 5,
    LessThanEqual: 6,
    In: 7,
    NotIn: 8,
    Contains: 9,
    StartWith: 10,
    EndWith: 11,
    Like: 12,
    NotLike: 13,
    IsSpecified: 14,
    NotSpecified: 15,
    Between: 16
};

const noneComparer = () => {
    return true;
};

const boolArray: Array<any> = ["true", "True", "1", 1, true];
const boolFalseArray: Array<any> = ["false", "False", "0", 0, false];

const equalComparer = (value1: any | Array<any>, value2: any | Array<any>) => {
    const val = _.isArray(value2) ? value2[0]: value2;
    if (typeof value1 === "number" && typeof val === "string") {
        return String(value1) === val;
    }

    if (value1 && value1.constructor === Array && IsContain(value1, val)) {
        return true;
    }

    if (boolArray.indexOf(val) >= 0 && boolArray.indexOf(value1) >= 0) {
        return true;
    }

    if (boolFalseArray.indexOf(val) >= 0 && boolFalseArray.indexOf(value1) >= 0) {
        return true;
    }

    return value1 === val;
};

function IsContain(list: Array<any>, searchValue: string | number) {
    let iscontain = false;
    
    for (const option of list) {
    const value = option.Value;
        if (typeof searchValue === "number" && typeof value === "string") {
            if (String(searchValue) === value) {
                iscontain = true;
                break;
            }
        } 
        
        if (value === searchValue) {
            iscontain = true;
                break;
        }
    }

    return iscontain;
}

const greaterThan = (value1: number, value2: number | Array<number>) => {
    const val = _.isArray(value2) ? value2[0]: value2;
    return value1 > val;
};

const lesserThan = (value1: number, value2: number | Array<number>) => {
    const val = _.isArray(value2) ? value2[0]: value2;
    return value1 < val;
};

const notEqualComparer = (value1: any, value2: any) => {
    const val = _.isArray(value2) ? value2[0]: value2;
    if (typeof value1 === "number" && typeof val === "string") {
        return String(value1) !== val;
    }

    if (value1 && value1.constructor === Array && IsContain(value1, val)) {
        return false;
    }

    if (boolArray.indexOf(val) >= 0 && boolArray.indexOf(value1) >= 0) {
        return false;
    }

    if (boolFalseArray.indexOf(val) >= 0 && boolFalseArray.indexOf(value1) >= 0) {
        return false;
    }

    return value1 !== val;
};

const greaterEqual = (value1: any, value2: any) => {
    const val = _.isArray(value2) ? value2[0]: value2;
    return value1 >= val;
};

const lesserEqual = (value1: any, value2: any) => {
    const val = _.isArray(value2) ? value2[0]: value2;
    return value1 <= val;
};

const like = (value1: any, value2: any) => {
    return value2.indexOf(value1) !== -1;
};

const startsWith = (value1: any, value2: any) => {
    return value2.startsWith(value1) !== -1;
};

const inComparer = (searchValue: any, searchArray: any) => {
    return CollectionCompare(searchValue, searchArray, true);
};

const CollectionCompare = (searchValue: any, searchArray: any, isIn: any) => {
    let isContain = false;
    const w: any = window;
    if (!w.isIE11) {
        isContain = searchArray.indexOf(searchValue) > -1;

        if (!isContain && searchValue) {
            isContain = searchArray.indexOf(String(searchValue)) > -1;
        }

        return isIn ? isContain : !isContain;
    }

    for (const value of searchArray) {
        if (typeof searchValue === "number" && typeof value === "string") {
            if (String(searchValue) === value) {
                isContain = true;
                break;
            }
        } else if (value === searchValue) {
            isContain = true;
            break;
        }
    }

    return isIn ? isContain : !isContain;
};

const notInComparer = (searchValue: any, searchArray: any) => {
    return CollectionCompare(searchValue, searchArray, false);
};

const isSpecified = (value: any) => {
    return typeof value !== "undefined" && value !== null && (isNaN(value) || (value > 0));
};

const notSpecified = (value: any) => {
    return typeof value === "undefined" || value === null || value === "" || value === "-1" || value === "0";
};

const betweenComparer = (value1: any, value2: any) => {
    const valSpecified = isSpecified(value1);
    if (valSpecified) {
        return value1 >= value2[0] && value1 <= value2[1];
    }

    return valSpecified;
};

const comparerFunctions = (function () {

    const operatorFunctions: any = {
        add(operator: any, operatorFunc: any) {
            this[operator] = operatorFunc;
        },
        get(operator: number) {
            return this[operator];
        }
    };

    operatorFunctions.add(operators.None, noneComparer);
    operatorFunctions.add(operators.Equal, equalComparer);
    operatorFunctions.add(operators.GreaterThan, greaterThan);
    operatorFunctions.add(operators.LessThan, lesserThan);
    operatorFunctions.add(operators.NotEqual, notEqualComparer);
    operatorFunctions.add(operators.GreaterThanEqual, greaterEqual);
    operatorFunctions.add(operators.LessThanEqual, lesserEqual);
    operatorFunctions.add(operators.Like, like);
    operatorFunctions.add(operators.StartWith, startsWith);
    operatorFunctions.add(operators.In, inComparer);
    operatorFunctions.add(operators.NotIn, notInComparer);
    operatorFunctions.add(operators.IsSpecified, isSpecified);
    operatorFunctions.add(operators.NotSpecified, notSpecified);
    operatorFunctions.add(operators.Between, betweenComparer);

    return operatorFunctions;
}());

const criteriaEvaluator = {
    execute(operator: number, value: object, compareToValue: object) {
        const comparer = comparerFunctions.get(operator);
        if (!comparer) {
            return true;
        }

        return comparer(value, compareToValue);
    }
};

export { criteriaEvaluator, operators };