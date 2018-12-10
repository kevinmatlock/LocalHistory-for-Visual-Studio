12/10/2018 v2.1.3 (beta)
* Made the same vsix compatible with both VS 2017 and 2019 (initial version with minimal testing).

12/10/2018 v2.1.2 (beta)
* Added support for VS 2019+. Using AsyncPackage so VS won't complain. 
Notes
* this is a preliminary version and VS 2019 is currently in preview mode. Be careful.
* this release was not tested on older VS versions.

06/28/2018 v2.1.1 (beta)
* Fixed history not being saved when the file is not in a subdir relative to the sln file's dir. More info on this issue: https://github.com/LostAlloy/LocalHistory-for-Visual-Studio/issues/3

02/14/2018 v2.0.2
* Fixed internal diff not opening.
* Added option to force the internal diff tool to use a single window.

02/05/2018
First public release of v2.0.0
Main features
* settings page (Tools -> Options -> LocalHistory)
* external diff tool support
* labels (select the history item to add/change label, then use the L key)
* filtering for "labeled only" in the history panel
