using System;

class clsEmployee
{
    // Private fields
    private int _ID;
    private string _Name = string.Empty;

    //ID Property Declaration
    public int ID
    {
        //Get is use for Reading field
        get
        {
            return _ID;
        }

        //Set is use for writing field
        set
        {
            _ID = value;

        }

    }


    //Name Property Declaration

    public string Name

    {
        //Get is use for Reading field
        get

        {
            return _Name;
        }

        //Set is use for writing field
        set
        {
            _Name = value;
        }

    }


    static void Main(string[] args)

    {

        //Create an object of Employee class.

        clsEmployee Employee1 = new clsEmployee();

        Employee1.ID = 7;

        Employee1.Name = "Mohammed Abu-Hadhoud";

        Console.WriteLine("Employee Id:={0}", Employee1.ID);

        Console.WriteLine("Employee Name:={0}", Employee1.Name);

        Console.ReadLine();

    }

}
