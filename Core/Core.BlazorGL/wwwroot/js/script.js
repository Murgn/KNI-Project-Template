function tickJS()
{
    window.theInstance.invokeMethod('TickDotNet');    
    window.requestAnimationFrame(tickJS);    
}

window.initRenderJS = (instance) =>
{
    window.theInstance = instance;

    // set initial canvas size 
    var canvas = document.getElementById('theCanvas');
    var holder = document.getElementById('canvasHolder');
    canvas.width = holder.clientWidth;
    canvas.height = holder.clientHeight;
    
    // disable context menu on right click
    canvas.addEventListener("contextmenu", e => e.preventDefault());

    // dragndrop
    ["dragenter", "dragover"].forEach(evt => {
        holder.addEventListener(evt, e => {
            e.preventDefault();
            e.dataTransfer.dropEffect = "copy";
            holder.classList.add("drag-over");
        });
    });

    ["dragleave", "drop"].forEach(evt => {
        holder.addEventListener(evt, e => {
            e.preventDefault();
            e.dataTransfer.dropEffect = "copy";
            holder.classList.remove("drag-over");
        });
    });

    holder.addEventListener("drop", async e => {
        const file = e.dataTransfer.files && e.dataTransfer.files[0];
        if (!file) return;

        const buffer = await file.arrayBuffer();
        const bytes = new Uint8Array(buffer);

        await window.theInstance.invokeMethodAsync('LoadRomFromBytes', bytes);
    });

    holder.addEventListener("dragover", e => e.preventDefault());
    holder.addEventListener("drop", e => e.preventDefault());
    
    // begin game loop
    window.requestAnimationFrame(tickJS);
    requestAnimationFrame(() => {
        window.dispatchEvent(new Event("resize"));
    });
};

window.addEventListener("keydown", function(event)
{
    // Prevent Arrows Keys and Spacebar scrolling the outer page
    // when running inside an iframe. e.g: itch.io embedding.
    if ([32, 37, 38, 39, 40].indexOf(event.keyCode) > -1)
        event.preventDefault();
});
window.addEventListener("wheel", function(event)
{
    // Prevent Mousewheel scrolling the outer page
    // when running inside an iframe. e.g: itch.io embedding.
    event.preventDefault();
}, { passive: false });