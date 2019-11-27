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
            string[] filesInDirectory = Directory.GetFiles(directory);
			long totalSize = 0;
            foreach (string element in filesInDirectory)
            {
				totalSize += GetFileSize(element);
            }

			return totalSize;
		}

		// Return the number of files (not counting directories) below the given directory
		public static int CountFiles(string directory)
		{
			string[] filesInDirectory = Directory.GetFiles(directory);
			int count = 0;
			foreach (string element in filesInDirectory)
			{
				count++;				
			}

			return count++;
		}

		// Return the nesting depth of the given directory. A directory containing only files (no subdirectories) has a depth of 0.
		public static int GetDepth(string directory)
		{
			string[] directoriesInDirectory = Directory.GetDirectories(directory);
			int depth = 0;
			int elementDepth;

			foreach (string element in directoriesInDirectory)
			{
				elementDepth = GetDepth(element) + 1;
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
			string[] elementsInDirectory = Directory.GetFileSystemEntries(directory);
			Tuple<string, long> smallestFile = new Tuple<string, long>(String.Empty, 0);
			Tuple<string, long> smallestFileInSubDir;
			long currentFileSize;

			foreach (string element in elementsInDirectory)
			{
				if (IsDirectory(element))
				{
					smallestFileInSubDir = GetSmallestFile(element);
					if (smallestFile.Item2 > smallestFileInSubDir.Item2 || smallestFile.Item2 == 0)
					{
						smallestFile = smallestFileInSubDir;
					}
				}
				else
				{
					currentFileSize = GetFileSize(element);
					if (smallestFile.Item2 > currentFileSize || smallestFile.Item2 == 0)
					{
						smallestFile = new Tuple<string, long>(element, currentFileSize);
					}
				}
			}

			return smallestFile;
		}

		// Get the path and size (in bytes) of the largest file below the given directory
		public static Tuple<string, long> GetLargestFile(string directory)
		{
			string[] elementsInDirectory = Directory.GetFileSystemEntries(directory);
			Tuple<string, long> largestFile = new Tuple<string, long>(String.Empty, 0);
			Tuple<string, long> largestFileInSubDir;
			long currentFileSize;

			foreach (string element in elementsInDirectory)
			{
				if (IsDirectory(element))
				{
					largestFileInSubDir = GetLargestFile(element);
					if (largestFile.Item2 < largestFileInSubDir.Item2 || largestFile.Item2 == 0)
					{
						largestFile = largestFileInSubDir;
					}
				}
				else
				{
					currentFileSize = GetFileSize(element);
					if (largestFile.Item2 < currentFileSize || largestFile.Item2 == 0)
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
			string[] elementsInDirectory = Directory.GetFileSystemEntries(directory);
			List<string> allFilesOfSize = new List<string>();

			foreach (string element in elementsInDirectory)
			{
				if (IsDirectory(element))
				{
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
