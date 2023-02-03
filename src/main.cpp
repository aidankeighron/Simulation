#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <iostream>
// settings
const unsigned int SCR_WIDTH = 1800;
const unsigned int SCR_HEIGHT = 900;
int main()
{
    // glfw: initialize and configure
    // ------------------------------
    glfwInit();
    glfwWindowHint(GLFW_SAMPLES, 4); // 4x antialiasing
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
    // glfw window creation
    // --------------------
    GLFWwindow *window = glfwCreateWindow(SCR_WIDTH, SCR_HEIGHT, "Simulation", NULL, NULL);
    if (window == NULL)
    {
        std::cout << "Failed to create GLFW window" << std::endl;
        glfwTerminate();
        return -1;
    }
    glfwMakeContextCurrent(window);
    // glad: load all OpenGL function pointers
    // ---------------------------------------
    if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
    {
        std::cout << "Failed to initialize GLAD" << std::endl;
        return -1;
    }
    // render loop
    // -----------
    while (!glfwWindowShouldClose(window))
    {
        glClear(GL_COLOR_BUFFER_BIT);
        glClearColor(0.3f, 0.3f, 0.3f,1.0f);
        // glfw: swap buffers and poll IO events (keypresses/released, mouse moved etc.)
        // ---------------------------------------------------
        glfwSwapBuffers(window);
        glfwPollEvents();
    }
    // glfw: terminate, clearing all previously allocated GLFWresources.
    //---------------------------------------------------------------
    glfwTerminate();
    return 0;
}