import nibabel as nib
import numpy as np

# Replace 'your_image_file.nii' with the path to your NIfTI file
#nifti_file_path = 'Assets/LumbarSpinePhantom_CT.nii'
nifti_file_path = "C:/Users/mdigr/4450_Capstone_AR_C-Arm_Sim/C-Arm Simulator/Assets/LumbarSpinePhantom_CT.nii"
nifti_image = nib.load(nifti_file_path)

# Get the data as a numpy array
image_data = nifti_image.get_fdata()

# Function to normalize a numpy array to a given range
def normalize_array(array, min_val, max_val):
    # Find the minimum and maximum values in the array
    array_min, array_max = array.min(), array.max()
    # Perform the normalization
    normalized_array = (array - array_min) / (array_max - array_min) * (max_val - min_val) + min_val
    return normalized_array

# Normalize the image data to 0 - 100
normalized_image_data = normalize_array(image_data, 0, 100)

# Function to save a 3D numpy array to a text file
def save_3d_array_to_text(array, file_path):
    # Open the file in write mode
    with open(file_path, 'w') as file:
        # Write array shape
        file.write('# Array shape: {0}\n'.format(array.shape))
        
        # Iterate through array
        for array_2d in array:
            # Write the 2D array (slice) to the file
            np.savetxt(file, array_2d, fmt='%-7.2f')
            # Write a separator for each slice
            file.write('# New slice\n')

# Specify the output text file path
output_text_file = 'output_3d_array.txt'
save_3d_array_to_text(image_data, output_text_file)
