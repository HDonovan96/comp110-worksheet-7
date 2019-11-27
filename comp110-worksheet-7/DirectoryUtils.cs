using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp110_worksheet_7
{
	public static class DirectoryUtils
	{
		// Return the size, in bytes, of the given file
		public static long GetFileSize(string filePath)
		{
			return new FileInfo(filePath).Length;
		}

		// Return true if the given path points to a directory, false if it points to a file
		public static bool IsDirectory(string path)
		{
			return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
		}

		// Return the total size, in bytes, of all the files below the given directory
		public static long GetTotalSize(string directory)
		{
			// Gets every element in 'directory', this includes files or sub-directories.
            string[] elementsInDirectory = Directory.GetFileSystemEntries(directory);
			long totalSize = 0;
			// Adds the size of each file to 'totalSize'
            foreach (string element in elementsInDirectory)
            {
				// If element is a directory calls function recursively.
				if (IsDirectory(element))
				{
					totalSize += GetTotalSize(element);
				}
				else
				{
					totalSize += GetFileSize(element);
				}	
            }

			return totalSize;
		}

		// Return the number of files (not counting directories) below the given directory
		public static int CountFiles(string directory)
		{
			// Gets every element in 'directory', this includes files or sub-directories.
			string[] elementsInDirectory = Directory.GetFileSystemEntries(directory);
			int fileCount = 0;

			foreach (string element in elementsInDirectory)
			{
				// If element is a directory calls function recursively.
				if (IsDirectory(element))
				{
					fileCount += CountFiles(element);
				}
				else
				{
					fileCount++;
				}
			}
			// Returns the number of elements in 'elementsInDirectory'.
			return fileCount;
		}

		// Return the nesting depth of the given directory. A directory containing only files (no subdirectories) has a depth of 0.
		public static int GetDepth(string directory)
		{
			// Gets every sub-directory in 'directory'.
			string[] directoriesInDirectory = Directory.GetDirectories(directory);
			int depth = 0;
			// Is used to store the result from recursive calls of 'GetDepth'.
			int elementDepth;

			// If there are any sub-directories under 'directory' returns at least one.
			foreach (string element in directoriesInDirectory)
			{
				elementDepth = GetDepth(element) + 1;
				// 'depth' is only overwritten if the depth of the sub-directory just checked was greater than the deepest directory already checked.
				if (elementDepth > depth)
				{
					depth = elementDepth;
				}
			}

			return depth;
		}

		// Get the path and size (in bytes) of the smallest file below the given directory
		public static Tuple<string, long> GetSmallestFile(string directory)
		{
			// Gets every element in 'directory', this includes files or sub-directories.
			string[] elementsInDirectory = Directory.GetFileSystemEntries(directory);
			Tuple<string, long> smallestFile = new Tuple<string, long>(String.Empty, 0);
			Tuple<string, long> smallestFileInSubDir;
			long currentFileSize;

			foreach (string element in elementsInDirectory)
			{
				// If the element is a directory then 'GetSmallestFile' is called recursively.
				// This means all sub-directories are also checked.
				if (IsDirectory(element))
				{
					smallestFileInSubDir = GetSmallestFile(element);
					// The ' smallestFile.Item2 == 0' is needed to stop this function always returning a value of zero. 
					if (smallestFile.Item2 > smallestFileInSubDir.Item2 || smallestFile.Item2 == 0)
					{
						smallestFile = smallestFileInSubDir;
					}
				}
				else
				{
					currentFileSize = GetFileSize(element);
					// The ' smallestFile.Item2 == 0' is needed to stop this function always returning a value of zero. 
					if (smallestFile.Item2 > currentFileSize || smallestFile.Item2 == 0)
					{
						smallestFile = new Tuple<string, long>(element, currentFileSize);
					}
				}
			}

			return smallestFile;
		}

		// Get the path and size (in bytes) of the largest file below the given directory
		// This function is functionally similar to 'GetSmallestFile', checking for a larger file size rather than a smaller one.
		public static Tuple<string, long> GetLargestFile(string directory)
		{
			// Gets every element in 'directory', this includes files or sub-directories.
			string[] elementsInDirectory = Directory.GetFileSystemEntries(directory);
			Tuple<string, long> largestFile = new Tuple<string, long>(String.Empty, 0);
			Tuple<string, long> largestFileInSubDir;
			long currentFileSize;

			foreach (string element in elementsInDirectory)
			{
				// If the element is a directory then 'GetLargestFile' is called recursively.
				// This means all sub-directories are also checked.
				if (IsDirectory(element))
				{
					largestFileInSubDir = GetLargestFile(element);
					if (largestFile.Item2 < largestFileInSubDir.Item2)
					{
						largestFile = largestFileInSubDir;
					}
				}
				else
				{
					currentFileSize = GetFileSize(element);
					if (largestFile.Item2 < currentFileSize)
					{
						largestFile = new Tuple<string, long>(element, currentFileSize);
					}
				}
			}

			return largestFile;
		}

		// Get all files whose size is equal to the given value (in bytes) below the given directory
		public static List<string> GetFilesOfSize(string directory, long size)
		{
			// Gets every element in 'directory', this includes files or sub-directories.
			string[] elementsInDirectory = Directory.GetFileSystemEntries(directory);
			List<string> allFilesOfSize = new List<string>();

			foreach (string element in elementsInDirectory)
			{
				if (IsDirectory(element))
				{
					// If the element is a directory then 'GetFilesOfSize' is called recursively.
					// This means all sub-directories are also checked.
					List<string> filesInSub = GetFilesOfSize(element, size);
					foreach (string file in filesInSub)
					{
						allFilesOfSize.Add(file);
					}
				}
				else
				{
					if (GetFileSize(element) == size)
					{
						allFilesOfSize.Add(element);
					}
				}
			}

			return allFilesOfSize;
		}
	}
}
