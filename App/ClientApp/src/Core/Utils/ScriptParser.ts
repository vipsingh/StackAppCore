import _ from "lodash";
const s = {"type":"Program","body":[{"type":"VariableDeclaration","declarations":[{"type":"VariableDeclarator","id":{"type":"Identifier","name":"x"},"init":{"type":"Literal","value":"xyz","raw":"\"xyz\""}}],"kind":"var"},{"type":"IfStatement","test":{"type":"BinaryExpression","operator":"!=","left":{"type":"Identifier","name":"x"},"right":{"type":"Literal","value":"zz","raw":"\"zz\""}},"consequent":{"type":"BlockStatement","body":[{"type":"ExpressionStatement","expression":{"type":"CallExpression","callee":{"type":"Identifier","name":"alert"},"arguments":[{"type":"Literal","value":"x","raw":"\"x\""}]}}]},"alternate":null}],"sourceType":"script"};

const vars: any = {};

export function parse() {
    //console.log(new Date());
    executeBody(s.body);
    //console.log(new Date());

    console.log(vars);
}

function executeBody(bodyColl: Array<any>) {
    _.each(bodyColl, (body) => {
        switch(body.type) {
            case "VariableDeclaration":
                VariableDeclaration(body.declarations);
                break;
            case "ExpressionStatement":
                ExpressionStatement(body.expression);
                break;
            case "IfStatement":
                IfStatement(body);
                break;
        }
    })
}

function VariableDeclaration(declartions: Array<any>) {
    const dec = declartions[0];
    const varNme= dec.id.name;
    let varVal = getVarFromExp(dec.init);    

    setVar(varNme, varVal);
}

function setVar(name: string, value: any) {
    vars[name] = value;
}

function getVar(name: string) {
    return vars[name];
}

function getVarFromExp(exp: any): any {
    if (exp.type === "Literal") {
        return LteralExp(exp);
    } else if (exp.type === "Identifier") {
        return getVar(exp.name);
    } else if (exp.type === "BinaryExpression") {
        return BinaryExpression(exp);
    } else if (exp.type === "CallExpression") {
        return CallExpression(exp as ICallExp);
    }
}

function ExpressionStatement(exp: IExp) {
    if (exp.type === "AssignmentExpression") {
        AssignmentExpression(exp as ISimpleExp);
    } else if (exp.type === "CallExpression") {
        CallExpression(exp as ICallExp);
    }
}

function LteralExp(exp: IValueExp) {
    return exp.value;
}

function BinaryExpression(expression: ISimpleExp) {
    const lVal = getVarFromExp(expression.left);
    const rVal = getVarFromExp(expression.right);

    return  _ExpFunc[expression.operator]([lVal, rVal]);
}

function AssignmentExpression(expression: ISimpleExp) {
    const lVal = expression.left.name;
    const rVal = getVarFromExp(expression.right);
    
    setVar(lVal, rVal);
}

function CallExpression(expression: ICallExp) {
    const fun = expression.callee.name;
    if (_CallFunc[fun]) {
        const args = _.map(expression.arguments, a => {
            return getVarFromExp(a);
        });

        return _CallFunc[fun].call(null, args);

    } else {
        throw new Error(`Function with name ${fun} is not defined`);
    }
}

function IfStatement(expression: IIfExp) {
    const test = BinaryExpression(expression.test);
    
    if (test) {
        executeBody(expression.consequent.body);
    }
}

const _ExpFunc: IDictionary<(args: Array<any>, binder?: any) => any> = {
    "+": (args: Array<any>, binder: any = null) => {
        let r = args[0];
        for(let i=1; i< args.length; i++)
            r += args[i];

        return r;
    },
    "-": (args: Array<any>, binder: any = null) => {
        let r = args[0];
        for(let i=1; i< args.length; i++)
            r -= args[i];
            
        return r;
    },
    "*": (args: Array<any>, binder: any = null) => {
        let r = args[0];
        for(let i=1; i< args.length; i++)
            r = r * args[i];
            
        return r;
    },    
    "/": (args: Array<any>, binder: any = null) => {
        let r = args[0];
        for(let i=1; i< args.length; i++)
            r = r / args[i];
            
        return r;
    },
    "==": (args: Array<any>, binder: any = null) => {                     
        return args[0] === args[1];
    },
    "!=": (args: Array<any>, binder: any = null) => {                     
        return args[0] !== args[1];
    },
    ">": (args: Array<any>, binder: any = null) => {
        return args[0] > args[1];
    }
}

const _CallFunc: IDictionary<(args: Array<any>, binder?: any) => any> = {
    "alert": (args: Array<any>, binder: any = null) => {
        console.log(`alert ${args}`);

        return args[0];
    }
} 


////////////////////////////////
interface IValueExp{
    type: string,
    value: any,
    name: string
}
interface IExp{
    type: string
}
interface ISimpleExp extends IExp {    
    operator: string,
    left: IValueExp,
    right: IValueExp
}

interface ICallExp extends IExp {
    callee: IValueExp,
    arguments: Array<IValueExp>
}

interface IBodyExp extends IExp {    
    body: Array<IExp>
}

interface IIfExp extends IExp {
    test: ISimpleExp,
    consequent: IBodyExp
}

setTimeout(() => {
    parse();
});