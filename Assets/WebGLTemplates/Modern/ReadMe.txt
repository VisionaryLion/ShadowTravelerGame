Thanks for purchase 'WebGL Template Modern'.

Require Unity 5.6 to work with the new embed API.

Get Started:

- Import the package in your unity project
- Go to Player Settings -> Resolution and Presentation -> Select 'Modern' as WebGL Template
- Ready!

Customize:

Change Fuxia default color (progress bar and state text): ---------------------------------------------------------------------------------------
- Open the 'style.css' file located in: WebGLTemplates -> Modern -> TemplateData -> style.css with you favorite html / css editor
- and change the color value of styles:

     .counter p -> color: #f60d54;
	 .counter .ProgressLine -> color: #f60d54;
	 .counter .color -> color: #f60d54;
	 
Change game window color: ----------------------------------------------------------------------------------------------------------------------
on the same script 'style.css', change in the style '.webgl-content .overlay' (default line 3)
background-color: #0d0d0d; with your hex color value or your customize background
	 
Change download / loading text: -----------------------------------------------------------------------------------------------------------------
- Open the 'UnityProgress.js' file located in: WebGLTemplates -> Modern -> TemplateData -> UnityProgress.js with you favorite html / css editor
- Change these lines:

     document.getElementById("loadingInfo").innerHTML = "loading";
	 document.getElementById("loadingInfo").innerHTML = "downloading";
	 
	 
Contact:
for any problem or question you can contac me on:

email: brinerjhonson.lc@gmail.com
forum: http://www.lovattostudio.com/forum/index.php