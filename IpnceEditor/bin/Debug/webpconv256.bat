set q=100
set f=60
for /D %%G in (*) do ffmpeg -y -framerate %f% -i "%%G/frame%%04d.png" -filter:v "crop=256:192:150:0,scale=256:192" -loop 0 -c:v libwebp_anim -r 30 -qscale %q% %%G.webp
