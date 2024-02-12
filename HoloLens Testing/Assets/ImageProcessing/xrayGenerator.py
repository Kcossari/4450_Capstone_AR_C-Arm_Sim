import numpy as np
import matplotlib.pyplot as plt
import nibabel as nib
import os
import time

# Function to normalize a numpy array to a given range
def normalize_array(array, min_val, max_val):
    # Find the minimum and maximum values in the array
    array_min, array_max = array.min(), array.max()
    # Perform the normalization
    normalized_array = (array - array_min) / (array_max - array_min) * (max_val - min_val) + min_val
    return normalized_array

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

def read_3d_array_from_text(file_path):
    with open(file_path, 'r') as file:
        shape_line = file.readline()
        shape = tuple(map(int, shape_line.strip().lstrip('# Array shape: ').strip('()').split(',')))
        slices = []
        slice_data = []
        for line in file:
            if line.startswith('# New slice'):
                if slice_data:
                    slices.append(np.array(slice_data))
                    slice_data = []
            else:
                slice_data.append(list(map(float, line.split())))
        if slice_data:
            slices.append(np.array(slice_data))
        array_3d = np.stack(slices, axis=0)
        return array_3d

def simulate_xray_with_rotation(voxel_array, angle_degrees):
    angle_degrees = angle_degrees % 360
    if 0 <= angle_degrees < 90:
        projected_image = np.sum(voxel_array, axis=0)
    elif 90 <= angle_degrees < 180:
        projected_image = np.sum(voxel_array, axis=1)
    elif 180 <= angle_degrees < 270:
        projected_image = np.sum(voxel_array, axis=2)
    else:
        projected_image = np.sum(voxel_array, axis=1)
        projected_image = np.flip(projected_image, axis=1)

    return projected_image

def create_circular_mask(shape, center=None, radius=None):
    if center is None:
        center = (int(shape[0]/2), int(shape[1]/2))
    if radius is None:
        radius = min(center[0], center[1], shape[0]-center[0], shape[1]-center[1])
    Y, X = np.ogrid[:shape[0], :shape[1]]
    dist_from_center = np.sqrt((X - center[1])**2 + (Y - center[0])**2)
    mask = dist_from_center <= radius
    return mask

def apply_circular_mask(image, mask):
    circular_image = np.zeros_like(image)
    circular_image[mask] = image[mask]
    return circular_image

def save_xray_to_text(xray_image, file_name):
    np.savetxt(file_name, xray_image, fmt='%d')

def save_xray_to_png(xray_image, file_name):
    plt.imshow(xray_image, cmap='gray')
    plt.axis('off')
    plt.savefig(file_name, bbox_inches='tight', pad_inches=0)
    plt.close()

# Replace 'your_image_file.nii' with the path to your NIfTI file
nifti_file_path = 'LumbarSpinePhantom_CT.nii'
nifti_image = nib.load(nifti_file_path)

# Get the data as a numpy array
image_data = nifti_image.get_fdata()

# Normalize the image data to 0 - 100
normalized_image_data = normalize_array(image_data, 0, 100)

# Specify the output text file path
output_text_file = 'output_3d_array.txt'
save_3d_array_to_text(image_data, output_text_file)

def main():
    # Wait until the output text file is created
    while not os.path.exists(output_text_file):
        time.sleep(1)  # Wait for 1 second before checking again
    
    text_file_path = 'output_3d_array.txt'
    voxel_array = read_3d_array_from_text(text_file_path)
    
    angle_degrees = 180  # Change as needed
    xray_image = simulate_xray_with_rotation(voxel_array, angle_degrees)
    mask = create_circular_mask(xray_image.shape)
    circular_xray_image = apply_circular_mask(xray_image, mask)
    
    # Save the circular-shaped X-ray as text and PNG
    save_xray_to_text(circular_xray_image, 'xray.txt')
    save_xray_to_png(circular_xray_image, 'xray.png')

if __name__ == "__main__":
    main()