# VERT-natHACKS2021
 VERT (Virtual Environment Rehabilitation Tool) is our submission for natHACKS 2021 in the Rehabilitation Stream.

# Hardware Requirements
<ul>
 <li>A PC with a GPU and 8gb+ Ram</li>
 <li>An Oculus Quest 2</li>
 <li>OpenBCI Ganglion + Gold Electrodes</li>
</ul>

# How To Setup
<ol>
 <li>Clone the repository</li>
 <li><a href="https://unity3d.com/get-unity/download">Install Unity</a> Hub</li>
 <li>In Unity Hub click "Add" and select the folder "Towers Of Hanoi - Oculus Quest 2 Unity Game"</li>
 <li>Once the project is added, click to run it in Unity v2020.3</li>
 <li>Plug your Quest 2 headset into the PC and enable Oculus Link.</li>
 <li>Setup your OpenBCI ganglion
   <ul>
   <li>Connect gold cup electrodes to pins 1-4, as well as ground and reference</li>
   <li>Place the electrodes on F7, Fp1, Fp2, F8 (Using the 10-20 system) with ground and reference on the ear lobes</li>
   <li>Plug the USB dongle into the PC. <a href="https://answers.microsoft.com/en-us/windows/forum/all/how-to-identify-com-ports-in-windows10/2591ed8b-805e-4e66-9513-836cdd49ed80">Find the COM Port Number</a> of the dongle and set the serial port in SimpleGetData.cs to match</li>
    <li>Power on the ganglion and run the Unity application. If everything works correctly a concentration value should be output to the console</li>
</ul>
 <li>Ready To Play! Post game statistics will automatically be uploaded to the web-based Dashboard. (Work In Progress)</li>
 <li>To run the dashboard locally:</li>
 <ul>
 <li>Open Command Prompt and cd to the Evaluation Portal folder</li>
 <li>Make sure you have npm installed. Run npm install to add all the required packages</li>
 <li>Run 'npm start' and a webpage should open in your browser.</li>
</ul>
 </li>
</ol>
