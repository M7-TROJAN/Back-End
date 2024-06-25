/*
We will learn about C# ternary operator and how to use it to control the flow of program.

Ternary operator are a substitute for if...else statement.

Why is it called ternary operator?

This operator takes 3 operand, hence called ternary operator.



The syntax of ternary operator is:

Condition ? Expression1 : Expression2;


The ternary operator works as follows:

If the expression stated by Condition is true, the result of Expression1 is returned by the ternary operator.
If it is false, the result of Expression2 is returned.

For example, we can replace the following code:
    if (number % 2 == 0)
    {
        isEven = true;
    }
    else
    {
        isEven = false;
    }
with the following code:
    isEven = (number % 2 == 0) ? true : false ;
*/