# SMB1 Area data parse algorithm

While I plan to overhaul the area data parsing algorithm, it's worth documenting since it's extremely complicated and tries to do a lot with as little space as possible.

When I originally tried to write an area data parser, there were many hidden mysteries as a result of all of the hidden tasks SMAS SMB1 does.

## Starting the algorithm

The area data parser starts at `$03:A5CC`. The renderer loads area data by first loading what is known as a _buffer data_. Buffer data is simply area data that is loaded elsewhere until it is ready to be loaded into the main area data memory. The details of this will be discussed below.

Buffer data is decoded when the end-of-level byte is reached, or the current buffer flag is set. There are five buffer flags at `$7E:1300`. Otherwise, the object's page flag is checked and the render page is updated if the flag is set.

Alternatively, if the page skip command is set, the render page is set by this instead. If the page skip command is set then we don't yet decode the buffer data.

If the current object is a scenery object, then we begin decoding the buffer data.

Otherwise, we determine whether the object page is behind the rendering page. If the object is ahead of the renderer, then the buffer data is decoded to area and area data is decoded to tilemap data. 

If an object is not ready to be decoded, the area data index i updated to the next object and that object is checked.