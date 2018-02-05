// Copyright 2017 LOSTALLOY
// Copyright 2013 Intel Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace LOSTALLOY.LocalHistory {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using JetBrains.Annotations;


    internal class DocumentRepository {

        #region Constructors and Destructors

        /// <summary>
        ///     Creates a new <code>DocumentRepository</code> for the given solution and repository.
        /// </summary>
        public DocumentRepository(string solutionDirectory, string repositoryDirectory) {
            SolutionDirectory = solutionDirectory;
            RepositoryDirectory = repositoryDirectory;

            if (!Directory.Exists(RepositoryDirectory)) {
                Directory.CreateDirectory(RepositoryDirectory);
            }

            File.SetAttributes(RepositoryDirectory, FileAttributes.Hidden);
        }


        /// <summary>
        ///     Creates a new new revision in the repository for the given project item.
        /// </summary>
        [CanBeNull]
        public DocumentNode CreateRevision(string filePath) {
            LocalHistoryPackage.Log("CreateRevision(" + filePath + ")");

            if (string.IsNullOrEmpty(filePath)) {
                return null;
            }

            filePath = Utils.NormalizePath(filePath);
            DocumentNode newNode = null;

            try {
                var dateTime = DateTime.Now;
                newNode = CreateRevisionNode(filePath, dateTime);
                if (newNode == null) {
                    return null;
                }

                // Create the parent directory if it doesn't exist
                if (!Directory.Exists(newNode.RepositoryPath)) {
                    LocalHistoryPackage.Log($"Creating (because it doesn't exist) \"{newNode.RepositoryPath}\"");
                    Directory.CreateDirectory(newNode.RepositoryPath);
                }

                // Copy the file to the repository
                LocalHistoryPackage.Log($"Copying \"{filePath}\" to \"{newNode.VersionFileFullFilePath}\"");
                File.Copy(filePath, newNode.VersionFileFullFilePath, true);

                if (Control == null) {
                    Control = (LocalHistoryControl)LocalHistoryPackage.Instance.ToolWindow?.Content;
                }

                if (Control?.LatestDocument.OriginalPath.Equals(newNode.OriginalPath) == true) {
                    Control.DocumentItems.Insert(0, newNode);
                }
            }
            catch (Exception ex) {
                LocalHistoryPackage.Log(ex.Message);
            }

            return newNode;
        }


        /// <summary>
        ///     Creates a new <see cref="DocumentNode" /> for the given file and time.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [CanBeNull]
        public DocumentNode CreateRevisionNode(string filePath, DateTime dateTime) {
            LocalHistoryPackage.Log(
                $"CreateRevisionNode(filePath:\"{filePath}\", dateTime:{dateTime}), SolutionDirectory:\"{SolutionDirectory}\", RepositoryDirectory:\"{RepositoryDirectory}\"");

            // ReSharper disable once MergeSequentialChecksWhenPossible
            if (string.IsNullOrEmpty(filePath) || !filePath.IsSubPathOf(SolutionDirectory)) {
                LocalHistoryPackage.Log("file \"{filePath}\" is not a sub-file of the sln dir. Will not create revision.");
                return null;
            }

            var originalFilePath = filePath;
            var fileName = Path.GetFileName(filePath);
            var repositoryPath =
                    Path.GetDirectoryName(filePath)
                        ?.Replace(
                            SolutionDirectory,
                            RepositoryDirectory,
                            StringComparison.InvariantCultureIgnoreCase) ?? RepositoryDirectory;

            LocalHistoryPackage.Log($"Path.GetDirectoryName(filePath):\"{Path.GetDirectoryName(filePath)}\"");
            LocalHistoryPackage.Log($"fileName:\"{fileName}\", repositoryPath:\"{repositoryPath}\"");

            return new DocumentNode(repositoryPath, originalFilePath, fileName, dateTime);
        }


        /// <summary>
        ///     Creates a <see cref="DocumentNode" /> for a given <see cref="versionedFullFilePath" />
        /// </summary>
        [CanBeNull]
        public DocumentNode CreateDocumentNodeForFilePath([CanBeNull] string versionedFullFilePath) {
            if (versionedFullFilePath == null) {
                return null;
            }

            LocalHistoryPackage.LogTrace($"Trying to create {nameof(DocumentNode)} for \"{versionedFullFilePath}\"");

            versionedFullFilePath = Utils.NormalizePath(versionedFullFilePath);
            string[] parts = Path.GetFileName(versionedFullFilePath).Split('$');
            var dateFromFileName = parts[0];
            var fileName = parts[1];
            var label = parts.Length == 3 ? parts[2] : null;

            var repositoryPath = Path.GetDirectoryName(versionedFullFilePath) ?? RepositoryDirectory;
            var originalFullFilePath =
                    Path.GetDirectoryName(versionedFullFilePath)
                        ?.Replace(
                            RepositoryDirectory,
                            SolutionDirectory,
                            StringComparison.InvariantCultureIgnoreCase) ?? SolutionDirectory;
            originalFullFilePath = Path.Combine(originalFullFilePath, fileName);

            LocalHistoryPackage.LogTrace(
                $"Creating {nameof(DocumentNode)} for \"{fileName}\" " +
                $"(repositoryPath:\"{repositoryPath}\", originalFullFilePath:\"{originalFullFilePath}\")"
            );

            return new DocumentNode(repositoryPath, originalFullFilePath, fileName, dateFromFileName, label);
        }

        #endregion


        #region Public Properties

        public string SolutionDirectory { get; set; }

        public string RepositoryDirectory { get; set; }

        // TODO: remove this
        public LocalHistoryControl Control { get; set; }

        #endregion


        #region Public Methods and Operators

        /// <summary>
        ///     Returns all DocumentNode objects in the repository for the given project item.
        /// </summary>
        public IEnumerable<DocumentNode> GetRevisions([CanBeNull] string filePath) {
            LocalHistoryPackage.Log($"Trying to get revisions for \"{filePath}\"");
            var revisions = new List<DocumentNode>();

            // ReSharper disable once MergeSequentialChecksWhenPossible
            if (string.IsNullOrEmpty(filePath) || !filePath.IsSubPathOf(SolutionDirectory)) {
                LocalHistoryPackage.Log(
                    $"filePath \"{filePath}\" is not a" +
                    $" sub-path of the solution directory \"{SolutionDirectory}\". Returning empty list.");
                return revisions;
            }

            var revisionsPath = Path.GetDirectoryName(filePath);
            revisionsPath =
                    revisionsPath?.Replace(
                        SolutionDirectory,
                        RepositoryDirectory,
                        StringComparison.InvariantCultureIgnoreCase) ?? RepositoryDirectory;
            var fileName = Path.GetFileName(filePath);

            if (!Directory.Exists(revisionsPath)) {
                LocalHistoryPackage.Log(
                    "revisionsPath does not exist." +
                    $" Returning empty list. \"{revisionsPath}\"");
                return revisions;
            }

            LocalHistoryPackage.Log($"Searching for revisions for \"{fileName}\" in \"{revisionsPath}\"");
            foreach (var fullFilePath in Directory.GetFiles(revisionsPath)) {
                var normalizedFullFilePath = Utils.NormalizePath(fullFilePath);
                string[] splitFileName = normalizedFullFilePath.Split('$');
                normalizedFullFilePath = $"{splitFileName[0]}{splitFileName[1]}";//remove the label part

                //when running the OnBeforeSave, VS can return the filename as lower
                //i.e., it can ignore the file name's case.
                //Thus, the only way to retrieve everything here is to ignore case
                //Remember that Windows is case insensitive by default, so we can't really
                //have two files with names that differ only in case in the same dir.
                if (!normalizedFullFilePath.EndsWith(fileName, StringComparison.OrdinalIgnoreCase)) {
                    //LocalHistoryPackage.Log($"Not a revision:\"{normalizedFullFilePath}\"");
                    continue;
                }

                LocalHistoryPackage.LogTrace($"Found revision \"{fullFilePath}\"");
                revisions.Add(CreateDocumentNodeForFilePath(fullFilePath));
            }

            revisions.Reverse();

            return revisions;
        }

        #endregion

    }
}
