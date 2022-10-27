VAR choice = ""

It took me all day... #speaker:You #portrait:Player_FML #layout:left
I'm a triangle... #speaker:Triangle #portrait:TestTriangle #layout:right
I don't care...
-> question
=== question ===
What do you want?
    + [Nothing]
        ~ choice = "nothing"
        -> chosen
    + [Everything]
        ~ choice = "everything"
        -> chosen
    + [A banana] 
        ~ choice = "banana"
        -> chosen
        
=== chosen ===
You chose {choice}! Are you sure?
... #speaker:You #portrait:Player_Natural #layout:left
+ [Yes]
    okay...
    -> END
+ [No]
-> question

-> END



