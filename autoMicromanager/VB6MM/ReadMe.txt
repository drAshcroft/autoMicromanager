You may have to install the setup file twice to get the COM components working.  I have not figured out a workaround for this yet.

If you are working in a visual studio environment, the library is registered both as a reference and as components (Top Menu/ Project/ References /Micromanager_net)


You must use CreateObject to get this working as GetObject does not seem to work.