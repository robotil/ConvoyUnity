CC = g++

CFLAGS = -std=c++0x -fPIC -O -g

LDFLAGS = -shared

INCLUDES = -I/home/robil/Unity-Projects/ConvoyUnity/Assets/Scripts/velodyneScripts/velodyne_ICD/include/

TARGET = libvlp.so

LIBS = libvlpcommunication.so

SRCS = VLPCommunicationWrapper.cpp

OBJS = $(SRCS:.cpp=.o)

all: $(TARGET)

# $@ and $^, are the left and right sides of the :
$(TARGET): $(OBJS)
	$(CC) $(LDFLAGS) -o $@ $^ $(LIBS)

$(SRCS:.cpp=.o):%.o:%.cpp
	$(CC) $(CFLAGS) $(INCLUDES) -c $(SRCS)

#. PHONY rule keeps make from doing something with a file named clean.
.PHONY: clean
clean:
	-${RM} ${TARGET} ${OBJS} $(SRCS:.cpp=.d)

