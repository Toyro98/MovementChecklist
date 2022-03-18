using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MovementChecklist
{
    public partial class Form1 : Form
    {
        // Tracks the number of completed states. This could be used for autosplitting in ASL
        private int completedStates = 0;

        // Check the bottom of this file to see what "MemoryValue" is
        private MemoryValue<float> posX = new MemoryValue<float>();
        private MemoryValue<byte> state = new MemoryValue<byte>();
        private MemoryValue<float> speed = new MemoryValue<float>();
        private MemoryHelper memoryHelper;
        private MemoryAddress positionAddress;
        private MemoryAddress stateAddress;
        private MemoryAddress speedAddress;

        public void InitializeMemory()
        {
            // The last offset are identical on all versions
            positionAddress = new MemoryAddress(offset: 0xE8);
            stateAddress = new MemoryAddress(offset: 0x500);
            speedAddress = new MemoryAddress(offset: 0x4B8);

            memoryHelper = new MemoryHelper();
        }

        public void ReadMemory()
        {
            // Update Addresses
            positionAddress.Update(memoryHelper);
            stateAddress.Update(memoryHelper);
            speedAddress.Update(memoryHelper);

            // Update current values
            posX.current = memoryHelper.ReadMemory<float>(positionAddress.target);
            state.current = memoryHelper.ReadMemory<byte>(stateAddress.target);
            speed.current = memoryHelper.ReadMemory<float>(speedAddress.target);

            // If state is 0 then we are propably changing level
            // If our x position is -79.89810944f then we are in Main Menu
            if (state.current == 0 || posX.current == -79.89810944f)
            {
                // Return means that we stop the function here
                return;
            }

            // 50 is considering walking (1.8 km/h)
            // This is also to prevent it automatically checking 
            if (state.current == 1 && speed.current < 50f)
            {
                return;
            } 

            // Don't check if the current and old is the same
            if (state.current != state.old)
            {
                // Loop through all movements
                for (int i = 0; i < movements.Count; i++)
                {
                    // Check if current "i" is the same as the current state and if we haven't completed this state
                    if (movements[i].Id == state.current && movements[i].Completed == false)
                    {
                        // Set item checked and set it as completed
                        checkedListBox1.SetItemChecked(i, true);
                        movements[i].Completed = true;

                        // Increase value and update the form title text
                        completedStates += 1;
                        Text = "Movement Checklist (" + completedStates + "/" + movements.Count + ")";

                        // Break out of loop, we're done here :)
                        break;
                    }
                }
            }

            // Set old to current
            state.old = state.current;
        }
    }

    // Instead of creating variables like this..
    //
    // float currentSpeed = 0f;
    // float oldSpeed = 0f;
    //
    // We use this instead and specify what data type it is. Float, int, byte, and etc
    //
    // MemoryValue<float> speed = new MemoryValue<float>();
    // 
    // To compare you type speed.Current != speed.Old instead currentSpeed != oldSpeed
    public class MemoryValue<T>
    {
        public T current;
        public T old;
    }
}