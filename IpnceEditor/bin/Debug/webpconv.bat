set q=100
set f=60
for /D %%G in (*) do ffmpeg -y -framerate %f% -i "%%G/frame%%04d.png" -filter:v "crop=1024:768:150:0,scale=1024:768" -loop 0 -c:v libwebp_anim -r 30 -qscale %q% %%G.webp
