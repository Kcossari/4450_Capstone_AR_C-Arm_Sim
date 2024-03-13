import argparse
import nibabel as nib
import numpy as np
import matplotlib.pyplot as plt
from scipy.ndimage import rotate

# Function to parse command-line arguments for rotation angles
def parse_arguments():
    parser = argparse.ArgumentParser(description='Generate an X-ray image from a specified position.')
    parser.add_argument('--angle_x', type=float, default=0, help='Rotation angle around the x-axis in degrees')
    parser.add_argument('--angle_y', type=float, default=0, help='Rotation angle around the y-axis in degrees')
    parser.add_argument('--angle_z', type=float, default=0, help='Rotation angle around the z-axis in degrees')
    return parser.parse_args()

# Load and normalize the NIfTI file
nifti_file_path = 'LumbarSpinePhantom_CT.nii'
nifti_image = nib.load(nifti_file_path)
image_data = nifti_image.get_fdata()

def normalize_array(array, min_val, max_val):
    array_min, array_max = array.min(), array.max()
    normalized_array = (array - array_min) / (array_max - array_min) * (max_val - min_val) + min_val
    return normalized_array

normalized_image_data = normalize_array(image_data, 0, 100)

def rotate_3d_array(array, angle_x, angle_y, angle_z):
    # Rotate around x-axis
    rotated_x = rotate(array, angle_x, axes=(1, 2), reshape=False, mode='nearest')
    # Rotate around y-axis
    rotated_xy = rotate(rotated_x, angle_y, axes=(0, 2), reshape=False, mode='nearest')
    # Rotate around z-axis
    rotated_xyz = rotate(rotated_xy, angle_z, axes=(0, 1), reshape=False, mode='nearest')
    return rotated_xyz

def simulate_xray(voxel_array):
    # Simulate X-ray by projecting (summing) along the z-axis
    return np.sum(voxel_array, axis=2)

def save_xray_to_png(xray_image, file_name):
    plt.imshow(xray_image, cmap='gray')
    plt.axis('off')
    plt.savefig(file_name, bbox_inches='tight', pad_inches=0)
    plt.close()

def main():
    args = parse_arguments()
    
    rotated_array = rotate_3d_array(normalized_image_data, args.angle_x, args.angle_y, args.angle_z)
    xray_image = simulate_xray(rotated_array)
    
    save_xray_to_png(xray_image, 'xray.png')

if __name__ == "__main__":
    main()
