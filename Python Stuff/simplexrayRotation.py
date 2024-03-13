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

def main():
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
