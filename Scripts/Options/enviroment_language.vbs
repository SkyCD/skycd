'Dim I as Integer
With Me
 .LoadLabel "Here you can change language.", 10, 10,,,true
 .AddLine
 L=.LoadListBoxWithLabel ("Language file: ",  10, .LastY+10)
 do
   k=dir
   .lstList(L).AddItem k
 loop until k=""
End With