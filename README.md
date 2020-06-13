# DialogueSystem

This is a personal project in which I have developed a basic dialogue system with custom tools.
The project was created with Unity 2018.3.12f.

The system is divided in 3 basic elements or entities, all of which are Scriptable Objects. Each element can be created through the inspector context menu.

  * Conversations
  * Characters
  * Dialogue Sequences
  
A Conversation contains a list of the Characters that can be used in the conversation, as well as a list of the order of the dialogues and a reference to each character.

Characters can be assigned multiple Dialogue Sequences and have properties that allow for customization and easy identification, such as a name, color and icon.

Dialogue Sequences contain a series of Dialogues (text &| audio) which can be assigned to one or more characters.

As of this moment, I will focus on cleaning the code and create a better arquitecture for the custom editor scripts.

Plese report any bugs or any suggestions to: alvrz.aldo@gmail.com
