
Usage 1:
processor mergeSources {sourcesPath} {mergeFormat} {pathToFilesToMerge}

{sourcesPath}			Path where existing sources live
{mergeFormat}			Files to merge, one of the following options: oldverses,
						altafsir, tanzil, commonTranslation
{pathToFilesToMerge}	Path at which file(s) to merge reside. If it's a single
						file, then that file is merged. If it's a directory, 
						then all files in that directory are merged.

Usage 2:

processor {what to create} {sourcePath} {destinationPath}

{what to create}	What types of files to create, one of the following options: createall, createmarkdown, createlucenedocs
{sourcePath}		The path from where the source documents are read.
{destinationPath}	The path at which markdown or lucene index docs will be created. This should not be the same as the destinationPath.

Examples:

processor createall c:\repos\islamitexts-corpus c:\visualStudio\IslamiTextsMVC\app_data
