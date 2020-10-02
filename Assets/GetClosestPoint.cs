using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClosestPoint : MonoBehaviour
{


    // A structure to represent a Point in 2D plane 
    struct Point
    {
        public int x, y;
    };


    /* Following two functions are needed for library function qsort(). 
       Refer: http://www.cplusplus.com/reference/clibrary/cstdlib/qsort/ */

    // Needed to sort array of points according to X coordinate 
    int compareX(Point a, Point b)
    {
        Point p1 = a, p2 = b;
        return (p1.x - p2.x);
    }
    // Needed to sort array of points according to Y coordinate 
    int compareY(Point a, Point b)
    {
        Point p1 = a, p2 = b;
        return (p1.y - p2.y);
    }

    // A utility function to find the distance between two points 
    float dist(Point p1, Point p2)
    {
        return Mathf.Sqrt((p1.x - p2.x) * (p1.x - p2.x) +
                     (p1.y - p2.y) * (p1.y - p2.y)
                   );
    }

    // A Brute Force method to return the smallest distance between two points 
    // in P[] of size n 
    float bruteForce(Point[] P, int n)
    {
        float min = Mathf.Infinity;
        for (int i = 0; i < n; ++i)
            for (int j = i + 1; j < n; ++j)
                if (dist(P[i], P[j]) < min)
                    min = dist(P[i], P[j]);
        return min;
    }

    // A utility function to find a minimum of two float values 
    float min(float x, float y)
    {
        return (x < y) ? x : y;
    }


    // A utility function to find the distance between the closest points of 
    // strip of a given size. All points in strip[] are sorted according to 
    // y coordinate. They all have an upper bound on minimum distance as d. 
    // Note that this method seems to be a O(n^2) method, but it's a O(n) 
    // method as the inner loop runs at most 6 times 
    float stripClosest(Point[] strip, int size, float d)
    {
        float min = d;  // Initialize the minimum distance as d 

        // Pick all points one by one and try the next points till the difference 
        // between y coordinates is smaller than d. 
        // This is a proven fact that this loop runs at most 6 times 
        for (int i = 0; i < size; ++i)
            for (int j = i + 1; j < size && (strip[j].y - strip[i].y) < min; ++j)
                if (dist(strip[i], strip[j]) < min)
                    min = dist(strip[i], strip[j]);

        return min;
    }

    // A recursive function to find the smallest distance. The array Px contains 
    // all points sorted according to x coordinates and Py contains all points 
    // sorted according to y coordinates 
    // float closestUtil(Point[] Px, Point[] Py, int n)
    // {
    //     // If there are 2 or 3 points, then use brute force 
    //     if (n <= 3)
    //         return bruteForce(Px, n);

    //     // Find the middle point 
    //     int mid = n / 2;
    //     Point midPoint = Px[mid];


    //     // Divide points in y sorted array around the vertical line. 
    //     // Assumption: All x coordinates are distinct. 
    //     Point[] Pyl = new Point[mid];   // y sorted points on left of vertical line 
    //     Point[] Pyr = new Point[n - mid];  // y sorted points on right of vertical line 
    //     int li = 0, ri = 0;  // indexes of left and right subarrays 
    //     for (int i = 0; i < n; i++)
    //     {
    //         if (Py[i].x <= midPoint.x && li < mid)
    //             Pyl[li++] = Py[i];
    //         else
    //             Pyr[ri++] = Py[i];
    //     }

    //     // Consider the vertical line passing through the middle point 
    //     // calculate the smallest distance dl on left of middle point and 
    //     // dr on right side 
    //     float dl = closestUtil(Px, Pyl, mid);
    //     // float dr = closestUtil(Px + mid, Pyr, n - mid);

    //     // Find the smaller of two distances 
    //     // float d = min(dl, dr);

    //     // Build an array strip[] that contains points close (closer than d) 
    //     // to the line passing through the middle point 
    //     // Point[] strip = new Point[n];
    //     // int j = 0;
    //     // for (int i = 0; i < n; i++)
    //     //     if (Mathf.Abs(Py[i].x - midPoint.x) < d)
    //     //         strip[j] = Py[i]; j++;

    //     // // Find the closest points in strip.  Return the minimum of d and closest 
    //     // // distance is strip[] 
    //     // return stripClosest(strip, j, d);
    // }

    // The main function that finds the smallest distance 
    // This method mainly uses closestUtil() 
    // // float closest(Point[] P, int n)
    // // {
    // //     Point[] Px = new Point[n];
    // //     Point[] Py = new Point[n];
    // //     for (int i = 0; i < n; i++)
    // //     {
    // //         Px[i] = P[i];
    // //         Py[i] = P[i];
    // //     }
    // //     // quickSort(Px,0,n);
    // //     // P.
    // //     // qsort(Px, n, sizeof(Point), compareX);
    // //     // qsort(Py, n, sizeof(Point), compareY);

    // //     // Use recursive function closestUtil() to find the smallest distance 
    // //     return closestUtil(Px, Py, n);
    // }
    static int partition(int[] arr, int low,
                                   int high)
    {
        int pivot = arr[high];

        // index of smaller element 
        int i = (low - 1);
        for (int j = low; j < high; j++)
        {
            // If current element is smaller  
            // than the pivot 
            if (arr[j] < pivot)
            {
                i++;

                // swap arr[i] and arr[j] 
                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }

        // swap arr[i+1] and arr[high] (or pivot) 
        int temp1 = arr[i + 1];
        arr[i + 1] = arr[high];
        arr[high] = temp1;

        return i + 1;
    }


    /* The main function that implements QuickSort() 
    arr[] --> Array to be sorted, 
    low --> Starting index, 
    high --> Ending index */
    static void quickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {

            /* pi is partitioning index, arr[pi] is  
            now at right place */
            int pi = partition(arr, low, high);

            // Recursively sort elements before 
            // partition and after partition 
            quickSort(arr, low, pi - 1);
            quickSort(arr, pi + 1, high);
        }
    }
}
