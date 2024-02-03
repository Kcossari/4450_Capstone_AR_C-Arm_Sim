import numpy as np
import matplotlib.pyplot as plt

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

def resize_projection(proj, target_shape):
    """
    Resize projection to target shape by repeating or removing rows and columns.
    This is a very basic resizing method and may distort the image.
    """
    resized_proj = np.zeros(target_shape)
    for i in range(target_shape[0]):
        for j in range(target_shape[1]):
            orig_i = int(i * proj.shape[0] / target_shape[0])
            orig_j = int(j * proj.shape[1] / target_shape[1])
            resized_proj[i, j] = proj[orig_i, orig_j]
    return resized_proj

def interpolate_projections(proj1, proj2, angle, base_angle):
    # Ensure proj2 is resized to match proj1's shape for interpolation
    if proj1.shape != proj2.shape:
        proj2 = resize_projection(proj2, proj1.shape)
    
    # Calculate the weight for interpolation based on angle
    weight = (angle % 90) / 90.0
    if base_angle in [90, 270]:  # Invert weight when interpolating between 90 and 180 or 270 and 360 degrees
        weight = 1 - weight
    interpolated_image = proj1 * (1 - weight) + proj2 * weight
    return interpolated_image


def simulate_xray_with_real_rotation(voxel_array, angle_degrees):
    angle_degrees = angle_degrees % 360
    # Direct projections
    axial_proj = np.sum(voxel_array, axis=0)
    sagittal_proj = np.sum(voxel_array, axis=1)
    coronal_proj = np.sum(voxel_array, axis=2)
    
    # Determine which projections to interpolate based on angle
    if 0 <= angle_degrees < 90:
        return interpolate_projections(axial_proj, sagittal_proj, angle_degrees, 0)
    elif 90 <= angle_degrees < 180:
        return interpolate_projections(sagittal_proj, coronal_proj, angle_degrees, 90)
    elif 180 <= angle_degrees < 270:
        return interpolate_projections(coronal_proj, sagittal_proj, angle_degrees, 180)
    else:  # 270 to 360
        return interpolate_projections(sagittal_proj, axial_proj, angle_degrees, 270)

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

def save_xray_to_text(xray_image, angle_degrees):
    filename = f'xray_{angle_degrees}.txt'
    np.savetxt(filename, xray_image, fmt='%d')

def save_xray_to_png(xray_image, angle_degrees):
    filename = f'xray_{angle_degrees}.png'
    plt.imshow(xray_image, cmap='gray')
    plt.axis('off')
    plt.savefig(filename, bbox_inches='tight', pad_inches=0)
    plt.close()

def main():
    text_file_path = 'output_3d_array.txt'
    voxel_array = read_3d_array_from_text(text_file_path)
    
    for angle_degrees in range(0, 360, 45):  # Example, change as needed
        xray_image = simulate_xray_with_real_rotation(voxel_array, angle_degrees)
        mask = create_circular_mask(xray_image.shape)
        circular_xray_image = apply_circular_mask(xray_image, mask)
        
        # Save the circular-shaped X-ray as text and PNG with angle in filename
        save_xray_to_text(circular_xray_image, angle_degrees)
        save_xray_to_png(circular_xray_image, angle_degrees)

if __name__ == "__main__":
    main()