-- This script creates two views, 'VehicleDetailsView' and 'VehicleMasterDetailsView,' 
-- to provide different levels of vehicle details, from basic information to more comprehensive master details. 
-- The comments explain the purpose of each view and its use.





-- Section 1: Create the 'VehicleDetailsView' View
-- This view combines data from various tables to provide detailed vehicle information.

-- Create a view named 'VehicleDetailsView' based on a complex query
CREATE VIEW VehicleDetailsView AS (
    SELECT 
        VehicleDetails.ID,
        Makes.Make,
        MakeModels.ModelName,
        SubModels.SubModelName,
        Bodies.BodyName,
        VehicleDetails.Vehicle_Display_Name,
        VehicleDetails.Year,
        DriveTypes.DriveTypeName,
        VehicleDetails.Engine,
        VehicleDetails.Engine_CC,
        VehicleDetails.Engine_Cylinders,
        VehicleDetails.Engine_Liter_Display,
        FuelTypes.FuelTypeName,
        VehicleDetails.NumDoors
    FROM VehicleDetails
    LEFT JOIN SubModels ON SubModels.SubModelID = VehicleDetails.SubModelID
    LEFT JOIN MakeModels ON MakeModels.ModelID = SubModels.ModelID
    LEFT JOIN Makes ON Makes.MakeID = MakeModels.MakeID
    LEFT JOIN Bodies ON Bodies.BodyID = VehicleDetails.BodyID
    LEFT JOIN DriveTypes ON DriveTypes.DriveTypeID = VehicleDetails.DriveTypeID
    LEFT JOIN FuelTypes ON FuelTypes.FuelTypeID = VehicleDetails.FuelTypeID
);

-- Retrieve data from the 'VehicleDetailsView' view
SELECT * FROM VehicleDetailsView;


-- Section 2: Create the 'VehicleMasterDetailsView' View
-- This view enhances the previous view by including additional IDs for reference.

-- Create a view named 'VehicleMasterDetailsView' based on the enhanced query
CREATE VIEW VehicleMasterDetailsView AS (
    SELECT 
        VehicleDetails.ID,
        VehicleDetails.MakeID,
        Makes.Make,
        VehicleDetails.ModelID,
        MakeModels.ModelName,
        VehicleDetails.SubModelID,
        SubModels.SubModelName,
        VehicleDetails.BodyID,
        Bodies.BodyName,
        VehicleDetails.Vehicle_Display_Name,
        VehicleDetails.Year,
        VehicleDetails.DriveTypeID,
        DriveTypes.DriveTypeName,
        VehicleDetails.Engine,
        VehicleDetails.Engine_CC,
        VehicleDetails.Engine_Cylinders,
        VehicleDetails.Engine_Liter_Display,
        VehicleDetails.FuelTypeID,
        FuelTypes.FuelTypeName,
        VehicleDetails.NumDoors
    FROM VehicleDetails
    LEFT JOIN SubModels ON SubModels.SubModelID = VehicleDetails.SubModelID
    LEFT JOIN MakeModels ON MakeModels.ModelID = SubModels.ModelID
    LEFT JOIN Makes ON Makes.MakeID = MakeModels.MakeID
    LEFT JOIN Bodies ON Bodies.BodyID = VehicleDetails.BodyID
    LEFT JOIN DriveTypes ON DriveTypes.DriveTypeID = VehicleDetails.DriveTypeID
    LEFT JOIN FuelTypes ON FuelTypes.FuelTypeID = VehicleDetails.FuelTypeID
    ORDER BY VehicleDetails.ID
);

-- Retrieve data from the 'VehicleMasterDetailsView' view
SELECT * FROM VehicleMasterDetailsView;
