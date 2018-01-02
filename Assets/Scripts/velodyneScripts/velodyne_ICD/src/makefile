CC = g++

CFLAGS = -std=c++0x -fPIC -O -g

LDFLAGS = -shared

INCLUDES = -I/home/robil/Unity-Projects/ConvoyUnity/Assets/Scripts/velodyneScripts/velodyne_ICD/include/

TARGET = libvlpcommunication.so

LIBS = -lboost_system -lboost_thread

SRCS = VLPCommunication.cpp \
	VLPCommunication16.cpp \
	VLPCommunication32.cpp \
	Logger.cpp \
	Utilities.cpp \


OBJS=$(subst .cc,.o,$(SRCS))

all: $(TARGET)

$(TARGET): $(OBJS)
	$(CC) $(LDFLAGS) $(CFLAGS) $(INCLUDES) -o $(TARGET) $(OBJS) $(LIBS)

VLPCommunication.o: VLPCommunication.cpp VLPCommunication.h

VLPCommunication16.o: VLPCommunication16.cpp VLPCommunication16.h
	
VLPCommunication32.o: VLPCommunication32.cpp VLPCommunication32.h
	
Logger.o: Logger.cpp Logger.h
	
Utilities.o: Utilities.cpp Utilities.h
	
clean:
	$(RM) $(TARGET) *.o
