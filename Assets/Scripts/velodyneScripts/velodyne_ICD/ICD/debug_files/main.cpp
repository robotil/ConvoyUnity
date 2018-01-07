/**
* main.cpp
* Author: Binyamin Appelbaum
* Date: 13.12.17
*/

#include "Tester.h"
#include "Logger.h"
#include <sstream>
#include <boost/date_time/posix_time/posix_time.hpp>

int main() {
    std::stringstream ss;
    ss << "********************* starting " << __FILE__ << ". Time: " << boost::posix_time::microsec_clock::local_time() << " ******************";
    // LOG << 4;
    LOG(_ALWAYS_, ss.str());

    Tester t;

    ss.str("");
    ss << "********************* Ending " << __FILE__ << ". Time: " << boost::posix_time::microsec_clock::local_time();
    LOG(_ALWAYS_, ss.str());
}