import numpy as np

# # Create some sample data
# data = np.random.random((5, 3))  # A 5x3 array of random numbers between 0 and 1
#
# # Save the data to a text file
# np.savetxt('C:/Users/mdigr/4450_Capstone_AR_C-Arm_Sim/C-Arm Simulator/Assets/data.txt', data, fmt='%.2f', delimiter='\t')
#
# print("Data saved successfully.")

# importing required modules
import matplotlib.pyplot as plt

# creating plotting data
xaxis = [1, 4, 9, 16, 25, 36, 49, 64, 81, 100]
yaxis = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

# plotting
plt.plot(xaxis, yaxis)
plt.xlabel("X")
plt.ylabel("Y")

# saving the file.Make sure you
# use savefig() before show().
plt.savefig('C:/Users/mdigr/4450_Capstone_AR_C-Arm_Sim/C-Arm Simulator/Assets/squares.png')
