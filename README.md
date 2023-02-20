# PhotoOrganizer
Photo organizer is a tool that allows you to organize photos based on some directives and their metadata.

## Code Details
Framework: .NET 6.0
Design Pattern: WPF with MVVM

## How to use

###### Main window:
    In the main window you can select the source directory and the Target Directory;
    both of them are mandatory and I reccomend you to select different folders since the software will copy the images and not move them.
    
    Source Directory: This is the directory containing the photoes that you need to organize.
    Target Directory: This is the directory under wich you will find the photo after processing.
    
###### Settings:
    Here you can select what images you want to consider and how you want to organize them.

    - InputTab: In the input tab you can select what Image Format you want to be considered (Options are: JPEG, BMP, PNG or all of them) 
    and if the program will consider also subfolders (checkbox "Recursive")

    - OutputTab: Here you will find two sections:
      Organization Settings:
             This will give you the option to place photos in subfolders; 
             First select the Create Subfolders checkbox to see the options.                             
             The Naming Convention textbox will contain the way the program will name the subfolders,
             if you use image metadata (indicated with $MetadataToken") to name them they will automatically organized in different folders.
             The Folder Naming Option selector allows you to insert metadata in the folder name with the Add button.               
      Output Name Settings: 
            This section will allow you to choose how images will be renamed.
            Naming convention field works the same as the folder one.
            The Use Metadata checkbox is used to not use the image metadata to rename files.
            (If you put in the naming convention a metadata value and then remove the checkbox the data will be ignored)                            
            Image Metadata will allow you to add image metadata options to the naming convention.                            
            Other info will allow you to add some other info to the naming convention.
                            
           
