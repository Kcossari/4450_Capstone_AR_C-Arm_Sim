import numpy as np
import matplotlib.pyplot as plt

# Function to read 3D array from the text file
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

def rotate_point(point, angle, axis='z'):
    c, s = np.cos(angle), np.sin(angle)
    if axis == 'z':
        R = np.array([[c, -s, 0], [s, c, 0], [0, 0, 1]])
    elif axis == 'y':
        R = np.array([[c, 0, s], [0, 1, 0], [-s, 0, c]])
    elif axis == 'x':
        R = np.array([[1, 0, 0], [0, c, -s], [0, s, c]])
    else:
        raise ValueError("Axis must be 'x', 'y', or 'z'")
    return np.dot(R, point)

def simulate_xray_at_angle(voxel_array, angle_degrees, axis='z'):
    angle_radians = np.radians(angle_degrees)
    dims = voxel_array.shape
    output_dims = dims[1:] if axis == 'z' else (dims[0], dims[2]) if axis == 'y' else (dims[0], dims[1])
    xray_image = np.zeros(output_dims)
    
    for i in range(output_dims[0]):
        for j in range(output_dims[1]):
            if axis == 'z':
                ray_sum = 0
                for k in range(dims[0]):
                    x, y, z = rotate_point(np.array([i, j, k]), -angle_radians, axis=axis)
                    x, y, z = int(round(x)), int(round(y)), int(round(z))
                    if 0 <= x < dims[1] and 0 <= y < dims[2] and 0 <= z < dims[0]:
                        ray_sum += voxel_array[z, x, y]
                xray_image[i, j] = ray_sum
    
    return xray_image

# Read the 3D array from the text file
text_file_path = 'output_3d_array.txt'
voxel_array = read_3d_array_from_text(text_file_path)

# Adjust the size of the array if necessary, or ensure the array size matches expected dimensions
# For this example, let's proceed with the array as is

# Simulate X-ray
angle = 0  # Angle in degrees
xray_image = simulate_xray_at_angle(voxel_array, angle, axis='z')

# Plotting the simulated X-ray
plt.imshow(xray_image, cmap='gray')
plt.title(f'Simulated X-ray at {angle} degrees')
plt.axis('off')
plt.show()
