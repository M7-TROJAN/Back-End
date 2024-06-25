/*
In C#, here is how we can declare an array.

datatype[] arrayName;
Here,

dataType - data type like int, string, char, etc
arrayName - it is an identifier
Let's see an example,

int[] age;
Here, we have created an array named age. It can store elements of int type.

But how many elements can it store?

To define the number of elements that an array can hold, we have to allocate memory for the array in C#. For example,

declare an array
    int[] age;

allocate memory for array
    age = new int[5];
Here, new int[5] represents that the array can store 5 elements. We can also say the size/length of the array is 5.

Note: We can also declare and allocate the memory of an array in a single line. For example,
    int[] age = new int[5];
*/