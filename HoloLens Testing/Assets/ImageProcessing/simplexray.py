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

def simulate_xray_circle_shape(voxel_array):
    xray_image = np.sum(voxel_array, axis=0)
    
    # Creating a circular mask without skimage
    radius = min(xray_image.shape) // 2
    center = (xray_image.shape[0] // 2, xray_image.shape[1] // 2)
    Y, X = np.ogrid[:xray_image.shape[0], :xray_image.shape[1]]
    dist_from_center = np.sqrt((X - center[1])**2 + (Y - center[0])**2)
    mask = dist_from_center <= radius
    circular_xray = np.zeros_like(xray_image)
    circular_xray[mask] = xray_image[mask]
    
    return circular_xray

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
    xray_image = simulate_xray_circle_shape(voxel_array)
    
    # Saving the circular-shaped X-ray as text and PNG
    save_xray_to_text(xray_image, 'xray.txt')
    save_xray_to_png(xray_image, 'xray.png')

if __name__ == "__main__":
    main()
