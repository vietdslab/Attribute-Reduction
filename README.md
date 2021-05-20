The algorithm is implemented by Python for the Wrapper phase and C#  for the Filter phase with Window 7 operating system and Core I5 processor, 4GB ram where the output of Filter phase is the input of Wrapper Phase. Among the classifiers for numerical data such as KNN, SVM, and RF, we chose the KNN classifier with K = 10 and p = 2 since the classification time on mixed datasets in which datasets with real and integer numbers is faster than the rest of the classifiers. The dataset classification accuracy on KNN was the average accuracy with ten runs using the ten-fold cross-evaluation method. 
The main step
-	Step 1: Run C# with CSV file input to find the Filter reduct
 ![image](https://user-images.githubusercontent.com/84446339/118915698-b252db80-b957-11eb-984b-2fec7e2b316f.png)
-	Step 2: Run Python with Filter reduct input to find the Wrapper reduct with final reduct has the best classification accuracy 
![image](https://user-images.githubusercontent.com/84446339/118915716-c0086100-b957-11eb-9e1c-6fd2160c1ee8.png)

 

